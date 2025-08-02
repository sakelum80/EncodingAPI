using Microsoft.AspNetCore.Identity.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Encoding.WebApi.Models;
using System.ComponentModel.DataAnnotations;


namespace Encoding.WebApi.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/auth")]
    public class AuthController : Controller
    {
        private readonly IConfiguration _config;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IConfiguration config, ILogger<AuthController> logger)
        {
            _config = config;
            _logger = logger;
        }

        [HttpPost("token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GenerateToken([FromBody] Models.LoginRequest request)
        {
            // Automatic model validation
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid request: {Errors}", ModelState.Values);
                return BadRequest(ModelState);
            }

            // Manual email format validation (redundant but secure)
            if (!new EmailAddressAttribute().IsValid(request.Email))
            {
                _logger.LogWarning("Invalid email format: {Email}", request.Email);
                return BadRequest("Invalid email format");
            }

            // Authenticate user (replace with your real user store)
            if (!AuthenticateUser(request.Email, request.Password))
            {
                _logger.LogWarning("Authentication failed for {Email}", request.Email);
                return Unauthorized("Invalid credentials");
            }

            // Generate token
            var token = GenerateJwtToken(request.Email);
            return Ok(new { token });
        }

        private bool AuthenticateUser(string email, string password)
        {
            return email == _config["Login:Email"] && password == _config["Login:Password"];
        }

        private string GenerateJwtToken(string email)
        {
            var securityKey = new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            var credentials = new SigningCredentials(
                securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(ClaimTypes.Role, "Encoder"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
