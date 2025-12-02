using MonitoringSolution.DTOs;
using MonitoringSolution.Models;
using MonitoringSolution.Repositories.Interfaces;
using MonitoringSolution.Services.Interfaces;
using Npgsql;

namespace MonitoringSolution.Services
{
    public class SensorService : ISensorService
    {
        private readonly ISensorRepository _repository;

        public SensorService(ISensorRepository repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// Метод сохранения данных с датчика.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task SaveSensorDataAsync(SensorInputDTO data)
        {
            await _repository.AddSensorDataAsync(data);
        }
        
        public async Task<IEnumerable<Sensor>> GetSensorDataAsync(int limit)
        {
            return await _repository.GetSensorsDataAsync(limit);
        }

        public async Task<IEnumerable<SensorSummary>> GetSensorsSummaryAsync()
        {
            // Коллекция с id датчиков, в проде их надо получать. 
            List<int> sensorIds = new List<int>() { 1, 2, 3 };
            
            var data = await _repository.GetSensorsSummaryAsync(sensorIds);
            var dataList = data.ToList();

                if (dataList.Any())
                {
                    foreach (var sensorSummary in dataList)
                    {
                        sensorSummary.Status = GetStatus(sensorSummary.LastValue);
                    }
                }

            return dataList.OrderBy(s => s.SensorId);
        }
        
        private string GetStatus(double? currentValue)
        {
            if (currentValue < 5 || currentValue > 90  ) return "ERROR";
            if (currentValue < 15 || currentValue > 70  ) return "WARNING";
            return "OK";
        }
    }
}
