using MonitoringSolution.DTOs;
using MonitoringSolution.Services.Interfaces;

namespace MonitoringSolution.Services
{
    public class SensorDataEmulator : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SensorDataEmulator> _logger;
        private readonly Random _random = new();

        public SensorDataEmulator(IServiceProvider serviceProvider, ILogger<SensorDataEmulator> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Эмулятор сигналов запущен.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var sensorService = scope.ServiceProvider.GetRequiredService<ISensorService>();

                    // Generate data for sensors 1, 2, 3
                    for (int sensorId = 1; sensorId < 4; sensorId++)
                    {
                        var value = _random.NextSingle() * 100;
                        await sensorService.SaveSensorDataAsync(new SensorInputDTO
                        {
                            SensorId = sensorId,
                            Value = value
                        });
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка эмулятора сигналов.");
                }

                // Ждем 1 секунду перед следующей генерацией
                await Task.Delay(1000, stoppingToken);
            }

            _logger.LogInformation("Эмулятор сигналов остановлен.");
        }
    }
}
