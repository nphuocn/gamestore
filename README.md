# GameStore API

> A .NET minimal API with Entity Framework Core that provides a REST API for managing a games catalog with persistent storage.

## Overview

- Project: GameStore.Api
- Tech: .NET 10 minimal API with Entity Framework Core
- Database: SQLite (default) or SQL Server (configurable)
- Purpose: CRUD endpoints for a games catalog with database persistence

## Features

- **Data Persistence**: Uses Entity Framework Core with SQLite/SQL Server
- **Validation**: FluentValidation for DTOs
- **Database Migrations**: Automated schema creation and updates
- **RESTful Endpoints**: Standard CRUD operations for games

## Endpoints

- `GET /games` — returns all games
- `GET /games/{id}` — returns a game by id (named route `GetGameById`)
- `POST /games` — creates a new game (returns `201 Created` using `CreatedAtRoute`)

DTOs are defined in `GameStore.Api/Dtos`.

## Running

1. Install .NET 10 SDK
2. From the project folder:

```powershell
cd GameStore.Api
dotnet build
dotnet run
```

The app will start on `https://localhost:7122` and expose the endpoints; use the `games.http` file in the `GameStore.Api` folder or any HTTP client to exercise the API.

## Configuration

### Connection Strings
The app supports multiple database providers configured in `appsettings.json`:

```json
"ConnectionStrings": {
  "GameStoreSql": "Server=(localdb)\\mssqllocaldb;Database=GameStoreDb;Trusted_Connection=True;MultipleActiveResultSets=true",
  "GameStoreSqlite": "Data Source=GameStore.db"
}
```

### Environment-specific Settings
- **Development**: See `appsettings.Development.json`
- **Production**: Override via environment variables or `appsettings.Production.json`

## Testing examples

- Use the included `games.http` file located at `GameStore.Api/games.http` (recommended for VS Code REST client extension)
- Or with `curl`:

```bash
curl https://localhost:7122/games

curl -X POST https://localhost:7122/games \
  -H "Content-Type: application/json" \
  -d '{"title":"New Game","genre":"Action","price":19.99,"releaseDate":"2026-01-01","developer":"Dev","publisher":"Pub"}'
```

## Notes

- **Database**: Data persists in SQLite database (`GameStore.db`) or configured database server
- **Migrations**: Entity Framework migrations are applied automatically on startup via `MigrateDb()`
- **Validation**: All POST requests are validated using FluentValidation
- **Response**: The POST endpoint returns `201 Created` with a Location header pointing to the newly created resource
- **Production Database**: 
	(`$env:ConnectionStrings__GameStoreSqlite="Data Source={prodcutionname}.db"`) 
	(`dotnet run`)