namespace Simulator.Sensors;

public class PressureSensor : Sensor
{
    public PressureSensor(string sensorName) : base(sensorName)
    {
        SensorType = "PressureSensor";
    }

    public override void GenerateNewValue()
    {
        Value = (float)Math.Cos(DateTime.Now.Second * Math.PI / (10^7));
    }
}