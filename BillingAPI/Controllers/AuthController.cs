using Microsoft.AspNetCore.Mvc;
using BillingAPI.Models;
using BillingAPI.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BillingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly BillingContext _context;
        private readonly IConfiguration _config;

        public AuthController(BillingContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("login")]
public IActionResult Login([FromBody] UserLogin model)
{
    var user = _context.Users.SingleOrDefault(u => u.Username == model.Username);
    if (user == null)
    {
        Console.WriteLine("User not found");
        return Unauthorized();
    }
    
    if (!BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
    {
        Console.WriteLine("Password verification failed");
        return Unauthorized();
    }

    var token = GenerateJwtToken(user);
    Console.WriteLine("Login successful, token generated");
    return Ok(new { token });
}


        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class UserLogin
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
