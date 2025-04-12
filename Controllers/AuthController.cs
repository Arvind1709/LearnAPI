using LearnAPI.AppDbContext;
using LearnAPI.AppStart;
using LearnAPI.Model;
using LearnAPI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LearnAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly BookNookDbContext _context;
        public AuthController(BookNookDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] ClientsModel user)
        {
            var User = await _context.Users.FirstOrDefaultAsync(x => x.Username == user.Username);

            if (User == null)
            {
                return Unauthorized(new { Message = "Invalid credentials" });
            }

            // 🔹 Compare hashed password
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(user.Password, User.PasswordHash);

            if (!isPasswordValid)
            {
                return Unauthorized(new { Message = "Invalid credentials" });
            }

            // ✅ Generate JWT token if password is valid
            // var token = JwtHelper.GenerateToken(User.Username, User.Role);

            // Dummy check - Replace with database validation
            if (User.Role == "Admin")
            {
                var token = JwtHelper.GenerateToken(user.Username, "Admin");
                return Ok(new { Token = token, UserId = User.Id });
            }
            else if (User.Role == "User")
            {
                var token = JwtHelper.GenerateToken(user.Username, "User");
                return Ok(new { Token = token });
            }

            return Unauthorized("Invalid user name or password");

            //return Ok(new { Token = token });
        }



        [Authorize(Roles = "Admin")]
        [HttpGet("protected")]
        [ServiceFilter(typeof(ActionLoggingFilter))]
        public IActionResult Protected()
        {
            var token = Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is missing");
            }
            var username = User.Identity?.Name; // Get username
            var role = User.FindFirst(ClaimTypes.Role)?.Value; // Get role from JWT
            return Ok(new { Message = $"You have accessed a protected endpoint. And User name is : {username} and role is :{role}" });
        }
    }
}
