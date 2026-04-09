# Alberta AQI Dashboard

A full-stack air quality monitoring dashboard for Alberta, Canada, focused on wildfire smoke tracking.

## Live Demo
http://alberta-aqi-prod.eba-mrnnuqw9.ca-central-1.elasticbeanstalk.com/

## Tech Stack
- **Frontend:** React + TypeScript + Tailwind CSS + React-Leaflet
- **Backend:** .NET 10 Web API + Entity Framework Core + Hangfire
- **Database:** PostgreSQL (local Docker / AWS RDS in production)
- **Infrastructure:** Docker + Docker Compose + AWS Elastic Beanstalk + AWS ECR

## Features
- Interactive Alberta station map with real-time PM2.5 color coding
- Official government stations and community sensors
- Automatic hourly data ingestion from OpenAQ API
- Wildfire smoke event detection *(coming soon)*
- Pollutant trend charts *(coming soon)*

## Data Source
[OpenAQ](https://openaq.org) — open air quality data platform

## Local Development

### Prerequisites
- Docker Desktop
- .NET 10 SDK
- Node.js 20+

### Run Locally
```bash
# Start the full stack
docker compose -f docker-compose.local.yml up -d

# Frontend available at http://localhost:5173
# Backend available at http://localhost:5223
# Hangfire dashboard at http://localhost:5223/hangfire
```

### Trigger Initial Data Sync
Use Swagger at `http://localhost:5223/swagger` to call:
1. `POST /api/ingestion/sync-stations`
2. `POST /api/ingestion/ingest-readings`

## Architecture
```
OpenAQ API → Hangfire Job → PostgreSQL → .NET Web API → React Frontend
```