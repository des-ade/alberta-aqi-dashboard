import type { Station } from '../types/station';
import { getAqiInfo } from '../utils/aqi';

interface Props {
  station: Station;
  pm25: number | null;
}

const StationPopup = ({ station, pm25 }: Props) => {
  const aqi = getAqiInfo(pm25);

  return (
    <div className="min-w-48">
      <h3 className="font-bold text-gray-900 text-sm mb-1">{station.name}</h3>

      {station.locality && (
        <p className="text-xs text-gray-500 mb-2">{station.locality}</p>
      )}

      <div className="flex items-center gap-2 mb-2">
        <span className={`text-xs px-2 py-0.5 rounded-full text-white ${aqi.tailwind}`}>
          {aqi.label}
        </span>
        <span className={`text-xs px-2 py-0.5 rounded-full ${
          station.isMonitor
            ? 'bg-blue-100 text-blue-700'
            : 'bg-gray-100 text-gray-600'
        }`}>
          {station.isMonitor ? 'Official' : 'Community'}
        </span>
      </div>

      {pm25 !== null ? (
        <p className="text-xs text-gray-700 font-medium">
          PM2.5: {pm25.toFixed(1)} µg/m³
        </p>
      ) : (
        <p className="text-xs text-gray-400">No PM2.5 data available</p>
      )}

      <p className="text-xs text-gray-400 mt-1">
        {station.isActive ? '🟢 Active' : '🔴 Inactive'}
      </p>

      {station.provider && (
        <p className="text-xs text-gray-400">Provider: {station.provider}</p>
      )}

      {station.datetimeLast && (
        <p className="text-xs text-gray-400">
          Last reading: {new Date(station.datetimeLast).toLocaleDateString()}
        </p>
      )}
    </div>
  );
};

export default StationPopup;