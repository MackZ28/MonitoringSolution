import React from 'react';
import {
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
  Alert,
  CircularProgress,
  Box
} from '@mui/material';
import { SensorSummary } from '../types/sensor';

interface SensorTableProps {
  data: SensorSummary[];
  loading: boolean;
  error?: string;
}

const SensorTable: React.FC<SensorTableProps> = ({ data, loading, error }) => {
  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleString();
  };

  if (loading) {
    return (
      <Box display="flex" justifyContent="center" alignItems="center" minHeight="200px">
        <CircularProgress />
      </Box>
    );
  }

  if (error) {
    return <Alert severity="error">{error}</Alert>;
  }

  return (
    <TableContainer component={Paper}>
      <Table sx={{ minWidth: 650 }} aria-label="sensor data table">
        <TableHead>
          <TableRow>
            <TableCell><strong>Sensor ID</strong></TableCell>
            <TableCell align="right"><strong>Min</strong></TableCell>
            <TableCell align="right"><strong>Max</strong></TableCell>
            <TableCell align="right"><strong>Average</strong></TableCell>
            <TableCell align="right"><strong>Value</strong></TableCell>
            <TableCell><strong>Last Update</strong></TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {data.map((sensor) => (
            <TableRow key={sensor.sensorId}>
              <TableCell component="th" scope="row">
                Sensor {sensor.sensorId}
              </TableCell>
              <TableCell align="right">{sensor.min.toFixed(2)}</TableCell>
              <TableCell align="right">{sensor.max.toFixed(2)}</TableCell>
              <TableCell align="right">{sensor.average.toFixed(2)}</TableCell>
              <TableCell align="right">{sensor.lastValue !== undefined ? sensor.lastValue.toFixed(2) : '-'}</TableCell>
              <TableCell>{formatDate(sensor.lastUpdate)}</TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
};

export default SensorTable;