using FileUpload.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
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
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
          if (_context.Movies == null)
          {
              return NotFound();
          }
            return await _context.Movies.Include(m => m.Country).ToListAsync();
        }

        // GET: api/Movie/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
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

            return movie;
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
        public async Task<ActionResult<MovieResponse>> PostMovie([FromForm]MovieDto request)
        {
            var fileName = await _iManageImage.UploadFile(request.Poster);
            
            var movie = new Movie
            {
                Name = request.Name,
                Description = request.Description,
                CountryId = request.CountryId,
                Genres = _context.Genres.Where(g => request.GenresIds.Contains(g.Id)).ToList(),
                Author = request.Author,
                ReleaseDate = request.ReleaseDate,
                Poster = fileName
            };
            
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            string countryName = _context.Countries.FirstOrDefault(c => c.Id == movie.CountryId).Name;
            
            var response = new MovieResponse(
                movie.Id, movie.Name, movie.Description, countryName, movie.ReleaseDate, 
                $"{_config.GetSection("Domain").Value}/Uploads/StaticContent/{movie.Poster}", movie.Author);

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
