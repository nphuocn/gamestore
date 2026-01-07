using FluentValidation;
using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        /// <summary>
        /// The Entity Framework database context for the GameStore application.
        /// Provides access to the database and allows for querying and manipulating data.
        /// </summary>
        private GameStoreContext _gameStoreContext;
        private readonly IValidator<NewGameDTO> _validator;

        public GameController(GameStoreContext context, IValidator<NewGameDTO> validator)
        {
            _gameStoreContext = context;
            _validator = validator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameDTO>>> GetGames()
        {
            // TODO: Implement logic to retrieve games
            var games = await _gameStoreContext.Games
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
            .ToListAsync();

            return Ok(games);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GameDTO>> GetGame(int id)
        {
            // TODO: Implement logic to retrieve a specific game
            var game = await _gameStoreContext.Games.Include(g => g.Genre).FirstOrDefaultAsync(g => g.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            return Ok(new GameDTO(
                        game.Id,
                        game.Title,
                        game.Genre!.Name,
                        game.Price,
                        game.ReleaseDate,
                        game.Developer,
                        game.Publisher
                        ));
        }

        [HttpPost]
        public async Task<ActionResult<GameDetailDTO>> CreateGame(NewGameDTO newGame)
        {
            // TODO: Implement logic to create a new game
            var validationResult = await _validator.ValidateAsync(newGame);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new { property = e.PropertyName, message = e.ErrorMessage });
                return BadRequest(errors);
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

            _gameStoreContext.Games.Add(newGameToAdd);
            await _gameStoreContext.SaveChangesAsync();

            GameDetailDTO createdGame = new(
                newGameToAdd.Id,
                newGameToAdd.Title,
                newGameToAdd.GenreId,
                newGameToAdd.Price,
                newGameToAdd.ReleaseDate,
                newGameToAdd.Developer!,
                newGameToAdd.Publisher
            );

            return CreatedAtAction(nameof(GetGame), new { id = 1 }, createdGame);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGame(int id, Game updatedGame)
        {
            // TODO: Implement logic to update a game
            var gameIndex = await _gameStoreContext.Games.FirstOrDefaultAsync(g => g.Id == id);
            if (gameIndex == null)
            {
                return NotFound();
            }

            gameIndex.Title = updatedGame.Title;
            gameIndex.GenreId = updatedGame.GenreId;
            gameIndex.Price = updatedGame.Price;
            gameIndex.ReleaseDate = updatedGame.ReleaseDate;
            gameIndex.Developer = updatedGame.Developer;
            gameIndex.Publisher = updatedGame.Publisher;

            _gameStoreContext.Games.Update(gameIndex);
            await _gameStoreContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            // TODO: Implement logic to delete a game            
            await _gameStoreContext.Games.Where(game => game.Id == id).ExecuteDeleteAsync();
            return NoContent();
        }
    }
}