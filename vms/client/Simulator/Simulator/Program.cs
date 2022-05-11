using System.Globalization;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using Microsoft.Extensions.Configuration;
using Simulator.Sensors;


var builder = new ConfigurationBuilder()
    .AddJsonFile("Configs/simulationsettings.json");
IConfigurationRoot configuration = builder.Build();

var sensorName = GetEnv("SIM_NAME", configuration);
var period = Int32.Parse(GetEnv("SIM_PERIOD", configuration));
var typeSim = GetEnv("SIM_TYPE", configuration);
var mqttBroker = GetEnv("SIM_HOST", configuration);
var mqttPort = Int32.Parse(GetEnv("SIM_PORT", configuration));

Console.WriteLine(sensorName);
Console.WriteLine(period);
Console.WriteLine(typeSim);
Console.WriteLine(mqttBroker);
Console.WriteLine(mqttPort);

Sensor sensor;

switch (typeSim)
{
    case "TemperatureSensor":
        sensor = new TemperatureSensor(sensorName);
        break;
    case "PressureSensor":
        sensor = new PressureSensor(sensorName);
        break;
    case "FlowSensor":
        sensor = new FlowSensor(sensorName);
        break;
    default:
        sensor = new TemperatureSensor(sensorName);
        break;
}

var mqttFactory = new MqttFactory();

using var mqttClient = mqttFactory.CreateMqttClient();
var mqttClientOptions = new MqttClientOptionsBuilder()
    .WithClientId(sensor.SensorName)
    .WithTcpServer(mqttBroker, mqttPort)
    .Build();

await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

Console.WriteLine("MQTT application message is published.");

while (true)
{
    sensor.GenerateNewValue();
    var applicationMessage = new MqttApplicationMessageBuilder()
        .WithTopic($"sensors/{sensor.SensorType}/{sensor.SensorName}")
        .WithPayload(sensor.Value.ToString(CultureInfo.InvariantCulture))
        .Build();

    await mqttClient.PublishAsync(applicationMessage, CancellationToken.None);
    Thread.Sleep(period);
}


string? GetEnv(string envVarName, IConfigurationRoot configurationRoot)
{
    var value = Environment.GetEnvironmentVariable(envVarName);
    Console.WriteLine($"Env {envVarName}: {value}");

    if (value is null)
    {
        // Environment.SetEnvironmentVariable("Test1", "Value1");
        // toDelete = true;
        //
        // // Now retrieve it.
        // value = Environment.GetEnvironmentVariable("Test1");
        
        return configuration.GetConnectionString(envVarName);
    }

    return value;
}