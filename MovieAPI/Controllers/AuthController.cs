using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MovieAPI.Models;

namespace MovieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        public static User user = new User();
        private readonly IConfiguration _configuration;
        private readonly MovieContext _movieContext;


        public AuthController(IConfiguration configuration, MovieContext movieContext)
        {
            _configuration = configuration;
            _movieContext = movieContext;
        }
        
        [HttpPost("register")]
        public ActionResult<User> Register(UserDto request)
        {
            string passwordHash
                = BCrypt.Net.BCrypt.HashPassword(request.Password);
            
            user.UserName = request.UserName;
            user.PasswordHash = passwordHash;
            user.Role = _movieContext.Roles.FirstOrDefault(role => role.Name == "Client");
            
            _movieContext.Users.Add(user);
            _movieContext.SaveChanges();
            
            return Ok(user);
        }
        
        [HttpPost("login")]
        public ActionResult<User> Login(UserDto request)
        {
            var user = _movieContext.Users.Where(u => u.UserName == request.UserName).FirstOrDefault();
            if (user == null)
            {
                return BadRequest("User not found");
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return BadRequest("Wrong password");
            }

            string token = CreateToken(user);
            
            return Ok(token);
        }

        private string CreateToken(User user)
        {
            string roleName = _movieContext.Roles.FirstOrDefault(r => r.Id == user.RoleId).Name;
            
            List<Claim> claims = new List<Claim>{
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, roleName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires:DateTime.Now.AddDays(1),
                    signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            
            return jwt;
        }
    }
}
