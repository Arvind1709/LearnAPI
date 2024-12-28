using LearnAPI.AppDbContext;
using LearnAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearnAPI.Controllers
{
    public class UsersController : ControllerBase
    {
        private readonly BookNookDbContext _context;

        public UsersController(BookNookDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return validation errors if the model is invalid.
            }

            // Example of password hashing (if applicable):
            // userModel.PasswordHash = HashPassword(userModel.PasswordHash);

            _context.Add(userModel);
            await _context.SaveChangesAsync();


            // Return 201 Created with the location of the created resource.
            return CreatedAtAction(nameof(GetUserById), new { id = userModel.Id }, userModel);
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
