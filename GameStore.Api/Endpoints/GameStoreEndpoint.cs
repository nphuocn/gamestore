using GameStore.Api.Dtos;
using FluentValidation;
using GameStore.Api.Data;
using Microsoft.EntityFrameworkCore;
using GameStore.Api.Models;
using GameStore.Api.Validators;

namespace GameStore.Api.Endpoints;

public static class GameStoreEndpoint
{
    const string GetGameByIdEndpointName = "GetGameById";
    const string UpdateGameEndpointName = "UpdateGame";

    public static void MapGameStoreEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/games");

        // GET /games
        group.MapGet("/", (GameStoreContext context) =>
        {
            return context.Games
            .Include(game => game.Genre)
            .Select(game => new GameDTO(
                        game.Id,
                        game.Title,
                        game.Genre!.Name,
                        game.Price,
                        game.ReleaseDate,
                        game.Developer,
                        game.Publisher
                    ))
            .AsNoTracking()
            .ToList();
        });

        // GET /games/{id}
        group.MapGet("/{id}", (int id, GameStoreContext context) =>
        {
            var game = context.Games.Include(g => g.Genre).FirstOrDefault(g => g.Id == id);
            return game is not null 
            ? Results.Ok(new GameDTO(
                        game.Id,
                        game.Title,
                        game.Genre!.Name,
                        game.Price,
                        game.ReleaseDate,
                        game.Developer,
                        game.Publisher
                    )) 
            : Results.NotFound();
        }).WithName(GetGameByIdEndpointName);

        // PUT /games/{id}
        group.MapPut("/{id}", async (int id, GameDetailDTO updatedGame, GameStoreContext context) =>
        {
            var gameIndex = await context.Games.FirstOrDefaultAsync(g => g.Id == id);
            if (gameIndex == null)
            {
                return Results.NotFound();
            }

            gameIndex.Title = updatedGame.Title;
            gameIndex.GenreId = updatedGame.GenreId;
            gameIndex.Price = updatedGame.Price;
            gameIndex.ReleaseDate = updatedGame.ReleaseDate;
            gameIndex.Developer = updatedGame.Developer;
            gameIndex.Publisher = updatedGame.Publisher;
            
            context.Games.Update(gameIndex);
            await context.SaveChangesAsync();

            return Results.AcceptedAtRoute(GetGameByIdEndpointName, new { id = updatedGame.Id }, updatedGame);
        }).WithName(UpdateGameEndpointName);

        // POST /games
        group.MapPost("/", async (NewGameDTO newGame, IValidator<NewGameDTO> validator, GameStoreContext context) =>
        {
            var validationResult = await validator.ValidateAsync(newGame);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new { property = e.PropertyName, message = e.ErrorMessage });
                return Results.BadRequest(new { errors });
            }

            Game newGameToAdd = new()
            {
                Title = newGame.Title,
                GenreId = newGame.GenreId,
                Price = newGame.Price,
                ReleaseDate = newGame.ReleaseDate,
                Developer = newGame.Developer,
                Publisher = newGame.Publisher
            };

            context.Games.Add(newGameToAdd);
            await context.SaveChangesAsync();

            GameDetailDTO createdGame = new(
                newGameToAdd.Id,    
                newGameToAdd.Title,
                newGameToAdd.GenreId,
                newGameToAdd.Price,
                newGameToAdd.ReleaseDate,
                newGameToAdd.Developer!,
                newGameToAdd.Publisher
            );

            return Results.CreatedAtRoute(GetGameByIdEndpointName, new { id = createdGame.Id }, createdGame);
        });

        // DELETE /games/{id}
        group.MapDelete("/{id}", async (int id, GameStoreContext context) =>
        {
            await context.Games.Where(game => game.Id == id).ExecuteDeleteAsync();
            return Results.NoContent();
        });
    }
}
