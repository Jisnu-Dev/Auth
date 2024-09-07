using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;


namespace JWTAuthDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;
        private static List<User> users = new List<User>
        {
            new User { Username = "testuser", Password = "password123" }
        };

        public AuthController(JwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost("signup")]
        public IActionResult SignUp([FromBody] User user)
        {
            if (users.Exists(u => u.Username == user.Username))
                return BadRequest("User already exists");

            users.Add(user);
            return Ok("User registered successfully");
        }

        [HttpPost("signin")]
        public IActionResult SignIn([FromBody] User login)
        {
            var user = users.FirstOrDefault(u => u.Username == login.Username && u.Password == login.Password);
            if (user == null)
                return Unauthorized("Invalid username or password");

            var token = _jwtService.GenerateToken(user.Username);
            return Ok(new { Token = token });
        }

        [HttpDelete("delete/{username}")]
        public IActionResult DeleteUser(string username)
        {
            var user = users.FirstOrDefault(u => u.Username == username);
            if (user == null)
                return NotFound("User not found");
            
            users.Remove(user);
            return Ok("User deleted successfully");
        }
    }

    public class User
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
}
