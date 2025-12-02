using MonitoringSolution.DTOs;
using MonitoringSolution.Models;

namespace MonitoringSolution.Repositories.Interfaces
{
    public interface ISensorRepository
    {
        Task AddSensorDataAsync(SensorInputDTO data);
        Task<IEnumerable<Sensor>> GetSensorsDataAsync(int limit);
        Task<IEnumerable<SensorSummary>> GetSensorsSummaryAsync(List<int> sensorIds);
    }
}
