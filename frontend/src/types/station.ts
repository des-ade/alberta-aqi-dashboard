export interface Sensor {
  id: number;
  parameter: string;
  displayName: string;
  units: string;
}

export interface Station {
  id: number;
  name: string;
  locality: string | null;
  latitude: number;
  longitude: number;
  isMonitor: boolean;
  isActive: boolean;
  provider: string | null;
  datetimeLast: string | null;
  sensors: Sensor[];
}