using LearnAPI.AppDbContext;
using LearnAPI.Model;
using LearnAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace LearnAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly BookNookDbContext _context;

        public UsersController(BookNookDbContext context)
        {
            _context = context;
        }
        //[Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] UserViewModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return validation errors if the model is invalid.
            }

            // Example of password hashing (if applicable):
            // userModel.PasswordHash = HashPassword(userModel.PasswordHash);
            var newUser = new UserModel
            {
                Username = userModel.Username,
                Email = userModel.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userModel.Password), // Hash password
                //Role = "Customer",
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                PhoneNumber = userModel.PhoneNumber,
                Role = JsonConvert.SerializeObject(userModel.Roles),
                DateCreated = DateTime.Now,
                IsActive = true
            };

            _context.Add(newUser);
            await _context.SaveChangesAsync();


            // Return 201 Created with the location of the created resource.
            return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, newUser);
        }

        // Example of a method for retrieving a user by ID (used by CreatedAtAction):
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

    }
}
