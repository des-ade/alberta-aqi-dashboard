import { useStations } from '../hooks/useStations';
import AlbertaMap from './AlbertaMap';

const Dashboard = () => {
  const { stations, pm25Map, loading, error } = useStations();

  const activeCount = stations.filter(s => s.isActive).length;
  const officialCount = stations.filter(s => s.isMonitor).length;
  const communityCount = stations.filter(s => !s.isMonitor).length;

  return (
    <div className="min-h-screen bg-gray-950 text-white">

      {/* Navbar */}
      <nav className="bg-gray-900 border-b border-gray-800 px-6 py-4">
        <div className="max-w-screen-xl mx-auto flex items-center justify-between">
          <div className="flex items-center gap-3">
            <span className="text-2xl">🔥</span>
            <div>
              <h1 className="text-lg font-bold text-white">Alberta AQI Dashboard</h1>
              <p className="text-xs text-gray-400">Wildfire Smoke Monitor — Powered by OpenAQ</p>
            </div>
          </div>
          <div className="text-xs text-gray-500">
            Data refreshes hourly
          </div>
        </div>
      </nav>

      {/* Stats Bar */}
      <div className="bg-gray-900 border-b border-gray-800 px-6 py-3">
        <div className="max-w-screen-xl mx-auto flex gap-6 text-sm">
          <div>
            <span className="text-gray-400">Total Stations </span>
            <span className="font-semibold text-white">{stations.length}</span>
          </div>
          <div>
            <span className="text-gray-400">Active </span>
            <span className="font-semibold text-green-400">{activeCount}</span>
          </div>
          <div>
            <span className="text-gray-400">Official </span>
            <span className="font-semibold text-blue-400">{officialCount}</span>
          </div>
          <div>
            <span className="text-gray-400">Community </span>
            <span className="font-semibold text-gray-300">{communityCount}</span>
          </div>
        </div>
      </div>

      {/* Main Content */}
      <main className="max-w-screen-xl mx-auto px-6 py-6">

        {/* Loading State */}
        {loading && (
          <div className="flex items-center justify-center h-96">
            <div className="text-center">
              <div className="animate-spin text-4xl mb-4">🌀</div>
              <p className="text-gray-400">Loading Alberta stations...</p>
            </div>
          </div>
        )}

        {/* Error State */}
        {error && (
          <div className="bg-red-900/30 border border-red-700 rounded-lg p-4 mb-6">
            <p className="text-red-400 font-medium">⚠️ {error}</p>
            <p className="text-red-500 text-sm mt-1">
              Make sure the backend is running on http://localhost:5223
            </p>
          </div>
        )}

        {/* Map */}
        {!loading && !error && (
          <div className="bg-gray-900 rounded-xl border border-gray-800 overflow-hidden">
            <div className="px-4 py-3 border-b border-gray-800 flex items-center justify-between">
              <h2 className="font-semibold text-gray-200">
                Alberta Air Quality Monitoring Stations
              </h2>
              <span className="text-xs text-gray-500">
                Larger circles = official government stations
              </span>
            </div>
            <div style={{ height: '680px' }}>
              <AlbertaMap stations={stations} pm25Map={pm25Map} />

            </div>
          </div>
        )}
      </main>
    </div>
  );
};

export default Dashboard;