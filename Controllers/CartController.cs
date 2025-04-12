using LearnAPI.AppDbContext;
using LearnAPI.Model;
using LearnAPI.ModelDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace LearnAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly BookNookDbContext _context;
        private readonly ILogger<CartController> _logger;

        public CartController(BookNookDbContext context, ILogger<CartController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost("add")]
        public IActionResult AddToCart([FromBody] AddToCartDto cartItem)
        {
            _logger.LogInformation("api/cart/add is called.");
            if (cartItem == null || cartItem.UserId <= 0 || cartItem.BookId <= 0 || cartItem.Quantity <= 0)
                return BadRequest("Invalid cart item data.");
            _logger.LogInformation("Invalid cart item data. is not fount");
            try
            {
                // Check if cart exists for the user
                _logger.LogInformation("Check if cart exists for the user");
                var cart = _context.Carts.FirstOrDefault(c => c.UserId == cartItem.UserId);
                if (cart == null)
                {
                    _logger.LogInformation("new cart will be created");
                    cart = new CartModel
                    {
                        UserId = cartItem.UserId,
                        CreatedDate = DateTime.UtcNow
                    };
                    _context.Carts.Add(cart);
                    _context.SaveChanges(); // Save to get CartId
                    _logger.LogInformation($"new cart  : {cart.Id} , is created");
                }
                else
                {
                    _logger.LogInformation($"Cart is already created with {cart.Id}");
                }

                // Check if the book already exists in the cart
                _logger.LogInformation("Check if the book already exists in the cart");
                var existingItem = _context.CartItems
                    .FirstOrDefault(ci => ci.CartId == cart.Id && ci.BookId == cartItem.BookId);

                if (existingItem != null)
                {
                    existingItem.Quantity += cartItem.Quantity;
                    _context.CartItems.Update(existingItem);
                }
                else
                {
                    var newCartItem = new CartItemModel
                    {
                        CartId = cart.Id,
                        BookId = cartItem.BookId,
                        Quantity = cartItem.Quantity
                    };
                    _context.CartItems.Add(newCartItem);
                }

                _context.SaveChanges();
                //return Ok("Book added to cart successfully.");
                return Ok(new { message = "Book added to cart successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding book to cart.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // CartController.cs
        //[HttpGet("GetCartByUserId/{userId}")]
        //public async Task<IActionResult> GetCartByUserId(int userId)
        //{
        //    var cart = await _context.Carts
        //        .Include(c => c.CartItems)
        //        //.ThenInclude(ci => ci.Book) // If you want book details
        //        .FirstOrDefaultAsync(c => c.UserId == userId);

        //    if (cart == null)
        //    {
        //        return NotFound("Cart not found for the user");
        //    }

        //    return Ok(cart);
        //}

        //[HttpGet("GetCartByUserId/{userId}")]
        //public async Task<IActionResult> GetCartByUserId(int userId)
        //{
        //    var cart = await _context.Carts
        //        .Include(c => c.CartItems)
        //        .Where(c => c.UserId == userId)
        //        .Select(c => new CartDto
        //        {
        //            Id = c.Id,
        //            UserId = c.UserId,
        //            CartItems = c.CartItems.Select(ci => new CartItemDto
        //            {
        //                Id = ci.Id,
        //                BookId = ci.BookId,
        //                Quantity = ci.Quantity
        //            }).ToList()
        //        })
        //        .FirstOrDefaultAsync();

        //    if (cart == null)
        //    {
        //        return NotFound("Cart not found for the user");
        //    }


        //    return Ok(cart);
        //}

        [HttpGet("GetCartByUserId/{userId}")]
        public async Task<IActionResult> GetCartByUserId(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Book) // Include book details
                .Where(c => c.UserId == userId)
                .Select(c => new CartDto
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    CartItems = c.CartItems.Select(ci => new CartItemDto
                    {
                        Id = ci.Id,
                        BookId = ci.BookId,
                        Quantity = ci.Quantity,
                        BookTitle = ci.Book.Title,        // Add Book Title
                        Price = ci.Book.Price,             // Add Book Price
                        BookCover = ci.Book.BookCover     // Add Book Cover Image
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (cart == null)
            {
                return NotFound("Cart not found for the user");
            }

            return Ok(cart);
        }

        // Add To Cart using Stored Procedure                   
        public void AddToCartSP(int userId, int bookId, int quantity)
        {
            var cmd = _context.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "AddToCart";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@UserId", userId));
            cmd.Parameters.Add(new SqlParameter("@BookId", bookId));
            cmd.Parameters.Add(new SqlParameter("@Quantity", quantity));

            _context.Database.OpenConnection();
            cmd.ExecuteNonQuery();
            _context.Database.CloseConnection();
        }

        // Get Cart using Stored Procedure 
        public async Task<IActionResult> GetCartByUserIdSP(int userId)
        {
            var result = await _context.CartItems
                .FromSqlRaw("EXEC GetCartByUserId @UserId = {0}", userId)
                .ToListAsync();

            if (result == null || !result.Any())
                return NotFound("Cart not found for the user");

            return Ok(result);
        }




    }

}
