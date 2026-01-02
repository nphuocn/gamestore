using GameStore.Api.Dtos;
using GameStore.Api.Validators;
using FluentValidation;

namespace GameStore.Api.Endpoints;

public static class GameStoreEndpoint
{
    const string GetGameByIdEndpointName = "GetGameById";
    const string UpdateGameEndpointName = "UpdateGame";
    private static List<GameDTO> games =
    [
        new GameDTO(1, "The Witcher 3: Wild Hunt", "RPG", 39.99m, new DateTime(2015, 5, 19), "CD Projekt Red", "CD Projekt"),
        new GameDTO(2, "Cyberpunk 2077", "RPG", 59.99m, new DateTime(2020, 12, 10), "CD Projekt Red", "CD Projekt"),
        new GameDTO(3, "God of War", "Action-Adventure", 49.99m, new DateTime(2018, 4, 20), "Santa Monica Studio", "Sony Interactive Entertainment"),
        new GameDTO(4, "Red Dead Redemption 2", "Action-Adventure", 59.99m, new DateTime(2018, 10, 26), "Rockstar Games", "Rockstar Games"),
        new GameDTO(5, "Minecraft", "Sandbox", 26.95m, new DateTime(2011, 11, 18), "Mojang Studios", "Mojang Studios")
    ];

    public static void MapGameStoreEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/games");

        // GET /games
        group.MapGet("/", () => games);

        // GET /games/{id}
        group.MapGet("/{id}", (int id) =>
        {
            var game = games.FirstOrDefault(g => g.Id == id);
            return game is not null ? Results.Ok(game) : Results.NotFound();
        }).WithName(GetGameByIdEndpointName);

        // PUT /games/{id}
        group.MapPut("/{id}", (int id, GameDTO updatedGame) =>
        {
            var gameIndex = games.FindIndex(game => game.Id == id);
            if (gameIndex == -1)
            {
                return Results.NotFound();
            }

            games[gameIndex] = updatedGame;
            return Results.AcceptedAtRoute(GetGameByIdEndpointName, new { id = updatedGame.Id }, updatedGame);
        }).WithName(UpdateGameEndpointName);

        // POST /games
        group.MapPost("/", async (NewGameDTO newGame, IValidator<NewGameDTO> validator) =>
        {
            var validationResult = await validator.ValidateAsync(newGame);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new { property = e.PropertyName, message = e.ErrorMessage });
                return Results.BadRequest(new { errors });
            }

            var newGameId = games.Max(g => g.Id) + 1;
            var newGameToAdd = new GameDTO(
                newGameId,
                newGame.Title,
                newGame.Genre,
                newGame.Price,
                newGame.ReleaseDate,
                newGame.Developer,
                newGame.Publisher
            );
            games.Add(newGameToAdd);
            return Results.CreatedAtRoute(GetGameByIdEndpointName, new { id = newGameId }, newGameToAdd);
        });

        // DELETE /games/{id}
        group.MapDelete("/{id}", (int id) =>
        {
            var gameIndex = games.FindIndex(game => game.Id == id);
            if (gameIndex == -1)
            {
                return Results.NotFound();
            }

            games.RemoveAt(gameIndex);
            return Results.NoContent();
        });
    }
}
