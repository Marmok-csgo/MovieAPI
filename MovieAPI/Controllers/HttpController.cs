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
        private readonly IConfiguration _configuration;

        public HttpController(MovieContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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
        [Route("Get/People")]

        public async Task<IActionResult> GetPeople()
        {
            
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://api.themoviedb.org/3/movie/554/credits?language=en-US"),
                Headers =
                {
                    { "accept", "application/json" },
                    { "Authorization", _configuration.GetSection("GetPeopleAuthorization").Value }
                }
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var jsonData = await response.Content.ReadAsStringAsync();
                
                var dataObject = JsonConvert.DeserializeObject<PeopleRootObject>(jsonData);

                List<string> peopleNames = dataObject.Cast.ConvertAll(item => item.Name);

                foreach (var name in peopleNames)
                {
                    if (_context.People.SingleOrDefault(p => p.Name == name) == null)
                    {
                        _context.People.Add(new People {Name = name});
                        _context.SaveChanges();
                    }
                }
            }
            return Ok();
        }
    }
}
