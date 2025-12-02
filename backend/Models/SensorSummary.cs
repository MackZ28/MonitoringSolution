namespace MonitoringSolution.Models
{
    public class SensorSummary
    {
        public int SensorId { get; set; }
        public double? Average { get; set; }
        public double? Max { get; set; }
        public double? Min { get; set; }
        public string Status { get; set; } = "OK";
        public double? LastValue { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
