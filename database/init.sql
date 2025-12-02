DROP TABLE IF EXISTS public.sensordata;
DROP TABLE IF EXISTS public.sensor_state;

-- Таблица с текущими данными с датчиков.
CREATE TABLE IF NOT EXISTS public.sensor_data (
    id SERIAL PRIMARY KEY,
    sensorid INT NOT NULL,
    "timestamp" TIMESTAMP DEFAULT CURRENT_TIMESTAMP NOT NULL,
    current_value FLOAT8 NOT NULL
);

CREATE INDEX IF NOT EXISTS idx_sensor_data_sensorid_ts ON public.sensor_data (sensorid, "timestamp" DESC);

-- Таблица с аггрегированными данными.
CREATE TABLE IF NOT EXISTS public.sensor_state (
    sensorid INT PRIMARY KEY,
    min_value FLOAT8,
    max_value FLOAT8,
    avg_value FLOAT8,
    current_value FLOAT8,
    last_updated TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);