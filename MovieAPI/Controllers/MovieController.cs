using FileUpload.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAPI.Models;
using MovieAPI.Responses;

namespace MovieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly MovieContext _context;
        private readonly IManageImage _iManageImage;
        private readonly IConfiguration _config;

        public MovieController(MovieContext context, IManageImage iManageImage, IConfiguration config)
        {
            _context = context;
            _iManageImage = iManageImage;
            _config = config;
        }

        // GET: api/films
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieResponse>>> GetMovies()
        {
            var movies = await _context.Movies
                .Include(m => m.Country)
                .Include(m => m.People)
                .Include(m => m.Genres)
                .ToListAsync();

            if (movies == null)
            {
                return NotFound();
            }

            var movieResponses = movies.Select(movie => new MovieResponse(
                movie.Id,
                movie.Name,
                movie.Description,
                movie.Country?.Name,
                movie.ReleaseDate,
                $"{_config.GetSection("Domain").Value}/Uploads/StaticContent/{movie.Poster}",
                movie.People?.FirstOrDefault(p => p.IsAuthor)?.Name, 
                movie.People?.Select(p => p.Name).ToList(), 
                movie.Genres?.Select(g => g.Name).ToList()
            )).ToList();

            return movieResponses;
        }


        // GET: api/Movie/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieResponse>> GetMovie(int id)
        {
            var movie = await _context.Movies
                .Include(m => m.Country)
                .Include(m => m.People)
                .Include(m => m.Genres)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound("Movie not found");
            }

            var movieResponse = new MovieResponse(
                movie.Id,
                movie.Name,
                movie.Description,
                movie.Country?.Name,
                movie.ReleaseDate,
                $"{_config.GetSection("Domain").Value}/Uploads/StaticContent/{movie.Poster}",
                movie.People?.FirstOrDefault(p => p.IsAuthor)?.Name, 
                movie.People?.Select(p => p.Name).ToList(), 
                movie.Genres?.Select(g => g.Name).ToList() 
            );

            return movieResponse;
        }

        

        // PUT: api/Movie/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutMovie(long id, Movie movie)
        {
            if (id != movie.Id)
            {
                return BadRequest();
            }

            _context.Entry(movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Movie
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        // [Authorize(Roles = "Admin")]
        public async Task<ActionResult<MovieResponse>> PostMovie([FromForm] MovieDto request)
        {
            var fileName = await _iManageImage.UploadFile(request.Poster);

            var genres = await _context.Genres.Where(g => request.GenresIds.Contains(g.Id)).ToListAsync();
            var people = await _context.People.Where(p => request.PeopleIds.Contains(p.Id)).ToListAsync();

            var country = await _context.Countries.FirstOrDefaultAsync(c => c.Id == request.CountryId);

            var movie = new Movie
            {
                Name = request.Name,
                Description = request.Description,
                Country = country,
                Genres = genres,
                People = people,
                ReleaseDate = request.ReleaseDate,
                Poster = fileName
            };

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            var response = new MovieResponse(
                movie.Id, movie.Name, movie.Description, country.Name, movie.ReleaseDate,
                $"{_config.GetSection("Domain").Value}/Uploads/StaticContent/{movie.Poster}",
                movie.People.FirstOrDefault(p => p.IsAuthor)?.Name,
                movie.People.Select(p => p.Name).ToList(),
                movie.Genres.Select(g => g.Name).ToList());

            return CreatedAtAction("GetMovie", new { id = movie.Id }, response);
        }


        // DELETE: api/Movie/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            if (_context.Movies == null)
            {
                return NotFound();
            }
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovieExists(long id)
        {
            return (_context.Movies?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
