using Microsoft.AspNetCore.Mvc;
using MovieAPI.Models;
using MovieAPI.Models.HttpApi;
using Newtonsoft.Json;
using Genre = MovieAPI.Models.Genre;

namespace MovieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HttpController : ControllerBase
    {
        
        private readonly MovieContext _context;

        public HttpController(MovieContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Get/Countries")]

        public async Task<IActionResult> GetCountry()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://countriesnow.space/api/v0.1/countries/positions")
            };

            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var jsonData  = await response.Content.ReadAsStringAsync();
                
                
                // Deserialize the JSON data into C# objects
                var dataObject = JsonConvert.DeserializeObject<RootObject>(jsonData);

                // Extract the "name" values
                List<string> countryNames = dataObject.Data.ConvertAll(item => item.Name);

                // Print the extracted names
                foreach (var name in countryNames)
                {
                    if (_context.Countries.SingleOrDefault(c => c.Name == name) == null)
                    {
                        _context.Countries.Add(new Country {Name = name});
                        _context.SaveChanges();
                    }
                }
            }

            return Ok();
        }

        [HttpGet]
        [Route("Get/Genres")]

        public async Task<IActionResult> GetGenre()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://api.themoviedb.org/3/genre/movie/list?language=en"),
                Headers =
                {
                    { "accept", "application/json" },
                    { "Authorization", "Bearer eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiI4ZDY3ODMzZTQ4ZDNmMjgwOGI5YWQzYWQ1MTUzN2Q0NCIsInN1YiI6IjY1NDA5M2RmNzUxMTBkMDEzOTYwYmQyYSIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.kAuIdFIawKWc4vSXfzhPZM1116P2NJX_Rid2i0kHYi4" },
                },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var jsonData = await response.Content.ReadAsStringAsync();
                
                var dataObject = JsonConvert.DeserializeObject<RootObj>(jsonData);
                
                List<string> genreNames = dataObject.genres.ConvertAll(item => item.Name);
                
                foreach (var name in genreNames)
                {
                    if (_context.Genres.SingleOrDefault(g => g.Name == name) == null)
                    {
                        _context.Genres.Add(new Genre {Name = name});
                        _context.SaveChanges();
                    }
                }
            }

            return Ok();
        }
    }
}
