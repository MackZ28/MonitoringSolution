using Microsoft.AspNetCore.Mvc;
using MonitoringSolution.DTOs;
using MonitoringSolution.Services.Interfaces;

namespace MonitoringSolution.Controllers
{
    [Route("api")]
    [ApiController]
    public class SensorController : ControllerBase
    {
        private readonly ISensorService _sensorService;

        public SensorController(ISensorService sensorService)
        {
            _sensorService = sensorService;
        }
        /// <summary>
        /// Сохранение данных с датчиков
        /// </summary>
        /// <param name="data">DTO с данными датчика</param>
        /// <returns></returns>
        [HttpPost("data")]
        public async Task<IActionResult> PostSensorData([FromBody] SensorInputDTO data)
        {
            try
            {
                await _sensorService.SaveSensorDataAsync(data);
                return Ok(new { message = "Data saved successfully" });
            }
            catch (Exception ex) {
                return StatusCode(500, new { error = ex.Message });
            }
        }
        /// <summary>
        /// Получение текущих данных с датчиков(по умолчанию поставил лимит - 100 элементов).
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet("data")]
        public async Task<IActionResult> GetSensorData([FromQuery] int limit = 100)
        {
            try
            {
                var result = await _sensorService.GetSensorDataAsync(limit);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
        /// <summary>
        /// Получение арегированных данных с датчиков(среднее, максимум, минимум).
        /// </summary>
        /// <returns></returns>
        [HttpGet("sensors/summary")]
        public async Task<IActionResult> GetSensorsSummary()
        {
            try
            {
                var summary = await _sensorService.GetSensorsSummaryAsync();
                return Ok(summary);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
