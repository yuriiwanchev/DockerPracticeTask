namespace Simulator.Sensors;

public class FlowSensor : Sensor
{
    public FlowSensor(string sensorName) : base(sensorName)
    {
        SensorType = "FlowSensor";
    }

    public override void GenerateNewValue()
    {
        Value = 1.1f + Value;
    }
}