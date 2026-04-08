import axios from 'axios';
import type { Station } from '../types/station';

// In Docker the nginx proxy handles routing so we use relative URLs
// In local dev we point directly to the backend port
const baseURL = import.meta.env.VITE_API_URL ?? '';

const api = axios.create({ baseURL });

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