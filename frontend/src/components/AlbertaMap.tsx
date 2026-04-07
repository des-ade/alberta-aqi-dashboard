import { MapContainer, TileLayer, CircleMarker, Popup } from 'react-leaflet';
import 'leaflet/dist/leaflet.css';
import '../utils/leafletIcons';
import type { Station } from '../types/station';
import { getAqiInfo } from '../utils/aqi';
import StationPopup from './StationPopup';
import AqiLegend from './AqiLegend';

interface Props {
  stations: Station[];
  pm25Map: Record<number, number>;
}

const ALBERTA_CENTER: [number, number] = [54.5, -114.5];
const DEFAULT_ZOOM = 6;

const AlbertaMap = ({ stations, pm25Map }: Props) => {
  return (
    <div className="relative w-full h-full">
      <MapContainer
        center={ALBERTA_CENTER}
        zoom={DEFAULT_ZOOM}
        className="w-full h-full rounded-lg"
        style={{ minHeight: '600px' }}
      >
        <TileLayer
          attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a>'
          url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
        />

        {[...stations]
          .sort((a, b) => {
            if (a.isMonitor === b.isMonitor) return 0;
            return a.isMonitor ? 1 : -1; // community renders first, official on top
          })
          .map(station => {
            const pm25 = pm25Map[station.id] ?? null;
            const aqi = getAqiInfo(pm25);
            const radius = station.isMonitor ? 10 : 7;

            // Community sensors with no PM2.5 data are heavily faded
            const opacity = !station.isMonitor && pm25 === null
              ? 0.15
              : station.isActive ? 0.9 : 0.4;

            return (
              <CircleMarker
                key={`${station.id}-${pm25 ?? 'null'}`}
                center={[station.latitude, station.longitude]}
                radius={radius}
                pathOptions={{
                  fillColor: aqi.color,
                  fillOpacity: opacity,
                  color: station.isMonitor ? '#1D4ED8' : '#6B7280',
                  weight: station.isMonitor ? 2 : 1,
                }}
              >
                <Popup maxWidth={220}>
                  <StationPopup station={station} pm25={pm25} />
                </Popup>
              </CircleMarker>
            );
          })
        }
      </MapContainer>

      <AqiLegend />
    </div>
  );
};

export default AlbertaMap;