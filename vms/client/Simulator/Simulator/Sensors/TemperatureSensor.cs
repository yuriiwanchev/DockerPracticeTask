namespace Simulator.Sensors;

public class TemperatureSensor : Sensor
{
    public TemperatureSensor(string sensorName) : base(sensorName)
    {
        SensorType = "TemperatureSensor";
    }

    public override void GenerateNewValue()
    {
        Value = 20 * (float)Math.Sin(DateTime.Now.Second * Math.PI / (10^7));
    }
}