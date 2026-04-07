const AqiLegend = () => {
  const bands = [
    { color: '#22C55E', label: 'Good (0–10 µg/m³)' },
    { color: '#EAB308', label: 'Moderate (11–24 µg/m³)' },
    { color: '#F97316', label: 'Sensitive Groups (25–35 µg/m³)' },
    { color: '#EF4444', label: 'Unhealthy (36–53 µg/m³)' },
    { color: '#A855F7', label: 'Very Unhealthy (54–150 µg/m³)' },
    { color: '#7F1D1D', label: 'Hazardous (150+ µg/m³)' },
    { color: '#9CA3AF', label: 'No Data' },
  ];

  return (
    <div className="absolute bottom-8 right-4 z-[1000] bg-white rounded-lg shadow-lg p-3 text-xs">
      <p className="font-semibold text-gray-700 mb-2">PM2.5 Air Quality</p>
      {bands.map(band => (
        <div key={band.label} className="flex items-center gap-2 mb-1">
          <div
            className="w-3 h-3 rounded-full flex-shrink-0"
            style={{ backgroundColor: band.color }}
          />
          <span className="text-gray-600">{band.label}</span>
        </div>
      ))}
      <div className="border-t border-gray-200 mt-2 pt-2 space-y-1">
        <div className="flex items-center gap-2">
          <span className="text-blue-600">●</span>
          <span className="text-gray-600">Official station</span>
        </div>
        <div className="flex items-center gap-2">
          <span className="text-gray-400">●</span>
          <span className="text-gray-600">Community sensor</span>
        </div>
      </div>
    </div>
  );
};

export default AqiLegend;