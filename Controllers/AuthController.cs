using LearnAPI.AppDbContext;
using LearnAPI.Model;
using LearnAPI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;

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
            var token = JwtHelper.GenerateToken(user.Username);
            return Ok(new { Token = token });
        }




        [Authorize]
        [HttpGet("protected")]
        public IActionResult Protected()
        {
            var token = Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is missing");
            }
            return Ok(new { Message = "You have accessed a protected endpoint." });
        }
    }
}
