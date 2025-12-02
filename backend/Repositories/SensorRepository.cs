using Dapper;
using MonitoringSolution.DTOs;
using MonitoringSolution.Models;
using MonitoringSolution.Repositories.Interfaces;
using Npgsql;

namespace MonitoringSolution.Repositories
{
    public class SensorRepository : ISensorRepository
    {
        private readonly NpgsqlDataSource _dataSource;

        public SensorRepository(NpgsqlDataSource dataSource)
        {
            _dataSource = dataSource;
        }

        public async Task AddSensorDataAsync(SensorInputDTO data)
        {
            try
            {
                using var connection = await _dataSource.OpenConnectionAsync();

                var sql = @"INSERT INTO public.sensor_data (sensorid, current_value)
                            VALUES (@SensorId, @Value);

                            -- 2. Обновляем агрегаты в sensor_state
                            WITH latest_window AS (
                                SELECT current_value
                                FROM public.sensor_data
                                WHERE sensorid = @SensorId
                                ORDER BY ""timestamp"" DESC
                                LIMIT @Limit
                            ),
                            combined AS (
                                SELECT current_value FROM latest_window
                                UNION ALL
                                SELECT @Value AS current_value  -- на случай, если @Limit = 1 и значение ещё не в истории
                            ),
                            stats AS (
                                SELECT
                                    MIN(current_value) AS new_min,
                                    MAX(current_value) AS new_max,
                                    AVG(current_value) AS new_avg
                                FROM combined
                            )
                            INSERT INTO public.sensor_state (
                                sensorid,
                                min_value,
                                max_value,
                                avg_value,
                                current_value,
                                last_updated
                            )
                            SELECT
                                @SensorId,
                                stats.new_min,
                                stats.new_max,
                                stats.new_avg,
                                @Value,
                                CURRENT_TIMESTAMP
                            FROM stats
                            ON CONFLICT (sensorid)
                            DO UPDATE SET
                                min_value = EXCLUDED.min_value,
                                max_value = EXCLUDED.max_value,
                                avg_value = EXCLUDED.avg_value,
                                current_value = EXCLUDED.current_value,
                                last_updated = EXCLUDED.last_updated;";

                await connection.ExecuteAsync(sql, new { data.SensorId, data.Value, data.Limit });

            } catch(NpgsqlException ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Sensor>> GetSensorsDataAsync(int limit)
        {
            try
            {
                using var connection = await _dataSource.OpenConnectionAsync();

                var sql = @"SELECT 
                                sensorid AS SensorId,
                                ""timestamp"" AS Timestamp,
                                current_value::REAL AS Value
                            FROM public.sensor_data
                            ORDER BY ""timestamp"" DESC
                            LIMIT @Limit"; 

                var data = await connection.QueryAsync<Sensor>(sql, new { Limit = limit });

                return data;

            } catch (NpgsqlException ex)
            {
                throw;
            }
            
        }

        public async Task<IEnumerable<SensorSummary>> GetSensorsSummaryAsync(List<int> sensorIds)
        {
            try
            {
                using var connection = await _dataSource.OpenConnectionAsync();

                var sql = @"SELECT 
                                sensorid AS SensorId,
                                min_value AS Min,
                                max_value AS Max,
                                avg_value AS Average,
                                current_value AS LastValue,
                                last_updated AS LastUpdate
                            FROM public.sensor_state
                            WHERE sensorid = ANY(@SensorIds)
                            ORDER BY sensorid";

                var data = await connection.QueryAsync<SensorSummary>(sql, new { SensorIds = sensorIds.ToArray()});

                return data;
            }
            catch (NpgsqlException ex)
            {
                throw;
            }
        }
    }
}
