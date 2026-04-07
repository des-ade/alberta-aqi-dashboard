import axios from 'axios';
import type { Station } from '../types/station';

// Points to your .NET backend
const api = axios.create({
  baseURL: 'http://localhost:5223',
});

export const fetchStations = async (activeOnly = false): Promise<Station[]> => {
  const response = await api.get<Station[]>('/api/stations', {
    params: { activeOnly },
  });
  return response.data;
};

export const fetchLatestPm25 = async (): Promise<Record<number, number>> => {
  const response = await api.get<Record<number, number>>('/api/stations/latest-pm25');
  return response.data;
};

export default api;