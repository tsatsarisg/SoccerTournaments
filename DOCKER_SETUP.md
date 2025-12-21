# Soccer Tournaments API - Docker Setup

## Prerequisites
- Docker and Docker Compose installed on your machine

## Starting the Database

### 1. Start PostgreSQL with Docker Compose
```bash
docker-compose up -d
```

This will:
- Pull the PostgreSQL 16 Alpine image
- Create a container named `soccer-tournaments-db`
- Expose PostgreSQL on `localhost:5432`
- Set default credentials: `postgres` / `postgres`

### 2. Verify the Database is Running
```bash
docker-compose ps
```

### 3. Connect to the Database (Optional)
```bash
docker exec -it soccer-tournaments-db psql -U postgres -d SoccerTournaments
```

## Running Migrations

Once the database is running, apply migrations from your .NET project:

```bash
cd src/SoccerTournaments.Api
dotnet ef database update --project ../SoccerTournaments.Teams
```

## Stopping the Database

```bash
docker-compose down
```

## Stopping and Removing All Data

```bash
docker-compose down -v
```

## Environment Variables

Connection string is configured in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=SoccerTournaments;Username=postgres;Password=postgres"
  }
}
```

## Troubleshooting

### Database connection fails
- Ensure Docker is running: `docker ps`
- Check if container is up: `docker-compose ps`
- Wait for health check to pass (may take 10-15 seconds)

### Port 5432 already in use
Change the port in `docker-compose.yml`:
```yaml
ports:
  - "5433:5432"  # Host port 5433 → Container port 5432
```
Then update connection string to use port 5433

### View logs
```bash
docker-compose logs postgres
```
