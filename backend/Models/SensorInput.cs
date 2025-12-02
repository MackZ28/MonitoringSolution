namespace MonitoringSolution.Models
{
    public class SensorInput
    {
        public int SensorId { get; set; }
        public float Value { get; set; }
        public int Limit { get; set; } = 100;
    }
}
