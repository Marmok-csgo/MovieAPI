using Microsoft.AspNetCore.Mvc;
using MovieAPI.Models;
using MovieAPI.Models.HttpApi;
using Newtonsoft.Json;

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
        [Route("Get")]

        public async Task<IActionResult> Get()
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
    }
}
