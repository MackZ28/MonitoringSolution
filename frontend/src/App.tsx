import React, { useState, useEffect, useCallback } from 'react';
import {
  Container,
  Typography,
  AppBar,
  Toolbar,
  Snackbar,
  Alert,
  Card,
  CardContent,
  Grid,
  Button // Добавляем кнопку для ручного обновления
} from '@mui/material';
import SensorTable from './components/SensorTable.tsx';
import { sensorAPI } from './services/api.ts';
import { SensorSummary } from './types/sensor.ts';

const App: React.FC = () => {
  const [sensorData, setSensorData] = useState<SensorSummary[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string>('');
  const [notification, setNotification] = useState({ open: false, message: '' });

  const fetchSensorData = useCallback(async () => {
    console.log('Fetching sensor data...');
    try {
      const summary = await sensorAPI.getSensorsSummary();
      console.log('Data fetched successfully:', summary);
      setSensorData(summary);
      setError('');
      
      setNotification({ open: true, message: `Data updated at ${new Date().toLocaleTimeString()}` });
    } catch (err) {
      const errorMessage = err instanceof Error ? err.message : 'Failed to fetch sensor data';
      console.error('Fetch error:', errorMessage);
      setError(errorMessage);
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    console.log('App mounted, starting interval...');
    fetchSensorData();
    
    const interval = setInterval(fetchSensorData, 5000);
    
    return () => {
      console.log('Clearing interval...');
      clearInterval(interval);
    };
  }, [fetchSensorData]);

  const handleCloseNotification = () => {
    setNotification({ ...notification, open: false });
  };

  return (
    <div className="App">
      <AppBar position="static">
        <Toolbar>
          <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
            Мониторинг датчиков
          </Typography>
          <Typography variant="body2" sx={{ mr: 2 }}>
            Данные обновляются каждые 5 секунд.
          </Typography>
          <Button 
            variant="outlined" 
            color="inherit"
            onClick={fetchSensorData}
            size="small"
          >
            Обновить сейчас
          </Button>
        </Toolbar>
      </AppBar>

      <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
        <Grid container spacing={3}>
          <Grid item xs={12}>
            <Card>
              <CardContent>
                <Typography variant="h5" component="h2" gutterBottom>
                  Данные с датчиков
                </Typography>
                <Typography variant="body2" color="textSecondary" paragraph>
                  Мониторинг данных с датчиков в реальном времени. Данные обновляются каждые 5 сек.
                </Typography>
                
                {/* Показываем отладочную информацию */}
                {error && (
                  <Alert severity="error" sx={{ mb: 2 }}>
                    Error: {error}
                  </Alert>
                )}
                
                <SensorTable data={sensorData} loading={loading} error={error} />
                
                {/* Отладочная информация */}
                <Typography variant="caption" color="textSecondary" sx={{ display: 'block', mt: 2 }}>
                  Data count: {sensorData.length}
                  {sensorData.length > 0 && ` | Last update: ${new Date(sensorData[0].lastUpdate).toLocaleString()}`}
                </Typography>
              </CardContent>
            </Card>
          </Grid>
        </Grid>
      </Container>

      <Snackbar 
        open={notification.open} 
        autoHideDuration={3000} 
        onClose={handleCloseNotification}
        anchorOrigin={{ vertical: 'bottom', horizontal: 'right' }}
      >
        <Alert onClose={handleCloseNotification} severity="success" sx={{ width: '100%' }}>
          {notification.message}
        </Alert>
      </Snackbar>
    </div>
  );
};

export default App;