using FluentValidation;
using GameStore.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GenreController : ControllerBase
    {
        /// <summary>
        /// The Entity Framework database context for the GameStore application.
        /// Provides access to the database and allows for querying and manipulating data.
        /// </summary>
        private GameStoreContext _gameStoreContext;

        public GenreController(GameStoreContext context)
        {
            _gameStoreContext = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GenreDTO>>> GetGenres()
        {
            // TODO: Implement logic to retrieve Genres
            var genres = await _gameStoreContext.Genres
            .Select(genre => new GenreDTO(
                        genre.Id,
                        genre.Name
                    ))
            .AsNoTracking()
            .ToListAsync();

            return Ok(genres);
        }
    }
}