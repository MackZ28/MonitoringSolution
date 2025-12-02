export interface SensorData {
  id: number;
  sensorId: number;
  timestamp: string;
  value: number;
}

export interface SensorSummary {
  sensorId: number;
  average: number;
  max: number;
  min: number;
  status: string; 
  lastValue: number;
  lastUpdate: string;
}