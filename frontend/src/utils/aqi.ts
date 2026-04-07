// Canadian AAQS PM2.5 AQI bands
export type AqiLevel = 'good' | 'moderate' | 'sensitive' | 'unhealthy' | 'very-unhealthy' | 'hazardous' | 'unknown';

export interface AqiInfo {
  level: AqiLevel;
  label: string;
  color: string;      // For map markers
  tailwind: string;   // For UI badges
}

export const getAqiInfo = (pm25: number | null): AqiInfo => {
  if (pm25 === null) return { level: 'unknown', label: 'No Data', color: '#9CA3AF', tailwind: 'bg-gray-400' };
  if (pm25 <= 10)   return { level: 'good',          label: 'Good',                     color: '#22C55E', tailwind: 'bg-green-500' };
  if (pm25 <= 24)   return { level: 'moderate',       label: 'Moderate',                 color: '#EAB308', tailwind: 'bg-yellow-500' };
  if (pm25 <= 35)   return { level: 'sensitive',      label: 'Sensitive Groups',          color: '#F97316', tailwind: 'bg-orange-500' };
  if (pm25 <= 53)   return { level: 'unhealthy',      label: 'Unhealthy',                color: '#EF4444', tailwind: 'bg-red-500' };
  if (pm25 <= 150)  return { level: 'very-unhealthy', label: 'Very Unhealthy',           color: '#A855F7', tailwind: 'bg-purple-500' };
  return              { level: 'hazardous',     label: 'Hazardous',                color: '#7F1D1D', tailwind: 'bg-red-900' };
};

// Extract PM2.5 latest value from a station's sensors
// We don't have latest values per sensor in the current API response
// so we return null for now — will be enhanced when readings are wired in
export const getStationPm25 = (): number | null => {
  return null;
};