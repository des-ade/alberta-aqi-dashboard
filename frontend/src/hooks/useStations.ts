import { useState, useEffect } from 'react';
import type { Station } from '../types/station';
import { fetchStations, fetchLatestPm25 } from '../services/api';

export const useStations = () => {
  const [stations, setStations] = useState<Station[]>([]);
  const [pm25Map, setPm25Map] = useState<Record<number, number>>({});
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const load = async () => {
      try {
        setLoading(true);
        const [data, pm25] = await Promise.all([
          fetchStations(),
          fetchLatestPm25(),
        ]);
        setStations(data);
        setPm25Map(pm25);
      } catch (err) {
        setError('Failed to load stations. Is the backend running?');
        console.error(err);
      } finally {
        setLoading(false);
      }
    };

    load();
  }, []);

  return { stations, pm25Map, loading, error };
};