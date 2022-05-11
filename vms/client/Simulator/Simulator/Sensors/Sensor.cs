namespace Simulator.Sensors;

public abstract class Sensor
{
    public string SensorType { get; init; }
    public string SensorName { get; } 
    public float Value { get; set; }
    
    protected Sensor(string sensorName)
    {
        SensorName = sensorName;
        Value = 0;
    }

    public abstract void GenerateNewValue();
}