namespace MonitoringSolution.DTOs
{
    public class SensorInputDTO
    {
        public int SensorId { get; set; }
        public float Value { get; set; }
        public int Limit { get; set; } = 100;
    }
}
