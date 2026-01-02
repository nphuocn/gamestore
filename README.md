# GameStore API

> A minimal example .NET minimal API that serves an in-memory list of games.

## Overview

- Project: GameStore.Api
- Tech: .NET 10 minimal API
- Purpose: Demo CRUD-style endpoints for a small games catalog (in-memory)

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

The app will start and expose the endpoints; use the `games.http` file in the `GameStore.Api` folder or any HTTP client to exercise the API.

## Testing examples

- Use the included `games.http` file located at `GameStore.Api/games.http` (recommended for VS Code REST client)
- Or with `curl` (replace host/port with the one the app reports):

```bash
curl http://localhost:5000/games

curl -X POST http://localhost:5000/games -H "Content-Type: application/json" -d '{"title":"New Game","genre":"Action","price":19.99,"releaseDate":"2026-01-01","developer":"Dev","publisher":"Pub"}'
```

## Notes

- Data is stored in-memory in `Program.cs` and resets on restart.
- The POST endpoint returns a `CreatedAtRoute` response that points to the `GetGameById` route.
