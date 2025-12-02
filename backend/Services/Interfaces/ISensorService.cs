using MonitoringSolution.DTOs;
using MonitoringSolution.Models;

namespace MonitoringSolution.Services.Interfaces
{
    public interface ISensorService
    {
        Task SaveSensorDataAsync(SensorInputDTO data);
        Task<IEnumerable<Sensor>> GetSensorDataAsync(int limit);
        Task<IEnumerable<SensorSummary>> GetSensorsSummaryAsync();
    }
}
