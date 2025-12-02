import { SensorData, SensorSummary } from '../types/sensor';

// ДЕБАГ: выводим URL который используем
console.log('REACT_APP_API_URL from env:', process.env.REACT_APP_API_URL);

const API_URL = process.env.REACT_APP_API_URL || 'http://localhost:5000';
console.log('Using API_URL:', API_URL);

export const sensorAPI = {
  async getSensorsSummary(): Promise<SensorSummary[]> {
    const url = `${API_URL}/api/sensors/summary`;
    console.log('Fetching from URL:', url);
    
    try {
      const response = await fetch(url);
      console.log('Response status:', response.status, response.statusText);
      
      if (!response.ok) {
        const errorText = await response.text();
        console.error('Response error text:', errorText);
        throw new Error(`HTTP ${response.status}: ${response.statusText}`);
      }
      
      const data = await response.json();
      console.log('Received data:', data);
      return data;
    } catch (error) {
      console.error('Fetch error details:', error);
      throw error;
    }
  },

  async getSensorData(): Promise<SensorData[]> {
    const url = `${API_URL}/api/data?sensorId=&limit=100`; // Как вариант, можно сделать дропдаун для выбора максимального количества записей.
    
    console.log('Fetching sensor data from:', url);
    const response = await fetch(url);
    if (!response.ok) {
      console.error('Failed to fetch sensor data:', response.status, response.statusText);
      throw new Error('Failed to fetch sensor data');
    }
    return response.json();
  },
};