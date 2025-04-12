using LearnAPI.AppDbContext;
using LearnAPI.Model;
using LearnAPI.ModelDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearnAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly BookNookDbContext _context;
        public OrderController(BookNookDbContext context)
        {
            _context = context;
        }
        [HttpPost("PlaceOrder")]
        public async Task<IActionResult> PlaceOrder([FromBody] CreateOrderDto orderDto)
        {
            //var cart = await _context.Carts
            //    .Include(c => c.CartItems)
            //    .ThenInclude(ci => ci.Book)
            //    .FirstOrDefaultAsync(c => c.UserId == orderDto.UserId);

            //if (cart == null || cart.CartItems == null || !cart.CartItems.Any())
            //{
            //    return BadRequest("Cart is empty or not found!");
            //}

            //// Calculate total price
            //decimal totalPrice = cart.CartItems.Sum(ci => ci.Quantity * ci.Book.Price);

            //// Create the order
            //var order = new OrderModel
            //{
            //    UserId = cart.UserId,
            //    OrderDate = DateTime.UtcNow,
            //    TotalAmount = totalPrice,
            //    OrderItems = cart.CartItems.Select(ci => new OrderItemModel
            //    {
            //        BookId = ci.BookId,
            //        Quantity = ci.Quantity,
            //        Price = ci.Book.Price
            //    }).ToList()
            //};

            //_context.Orders.Add(order);

            //// Optional: Clear the cart after order is placed
            //_context.CartItems.RemoveRange(cart.CartItems);
            //_context.Carts.Remove(cart); // If you want to remove the cart

            //await _context.SaveChangesAsync();

            //return Ok(new { Message = "Order placed successfully", OrderId = order.Id });

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Book)
                .FirstOrDefaultAsync(c => c.UserId == orderDto.UserId);

            if (cart == null || cart.CartItems == null || !cart.CartItems.Any())
            {
                return BadRequest("Cart is empty or not found!");
            }

            // Check stock availability before placing the order
            foreach (var item in cart.CartItems)
            {
                if (item.Book.Stock < item.Quantity)
                {
                    return BadRequest($"Not enough stock for book '{item.Book.Title}'. Available: {item.Book.Stock}");
                }
            }

            // Calculate total price
            decimal totalPrice = cart.CartItems.Sum(ci => ci.Quantity * ci.Book.Price);

            // Create the order
            var order = new OrderModel
            {
                UserId = cart.UserId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = totalPrice,
                OrderItems = cart.CartItems.Select(ci => new OrderItemModel
                {
                    BookId = ci.BookId,
                    Quantity = ci.Quantity,
                    Price = ci.Book.Price
                }).ToList()
            };

            _context.Orders.Add(order);

            // Reduce stock for each book
            foreach (var item in cart.CartItems)
            {
                item.Book.Stock -= item.Quantity;
            }

            // Optional: Clear the cart after order is placed
            _context.CartItems.RemoveRange(cart.CartItems);
            _context.Carts.Remove(cart); // If you want to remove the cart

            await _context.SaveChangesAsync();

            return Ok(new { Message = "Order placed successfully", OrderId = order.Id });
        }

        [HttpPost("shopNow")]
        public async Task<IActionResult> ShopNow([FromBody] ShopNowOrderDto orderDto)
        {
            if (orderDto == null || orderDto.UserId <= 0 || orderDto.BookId <= 0 || orderDto.Quantity <= 0)
                return BadRequest("Invalid order data.");

            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == orderDto.BookId);

            if (book == null)
                return NotFound("Book not found.");

            if (book.Stock < orderDto.Quantity)
                return BadRequest("Insufficient stock.");

            decimal totalPrice = book.Price * orderDto.Quantity;

            // Create new Order
            var newOrder = new OrderModel
            {
                UserId = orderDto.UserId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = totalPrice,
                OrderItems = new List<OrderItemModel>
        {
            new OrderItemModel
            {
                BookId = orderDto.BookId,
                Quantity = orderDto.Quantity,
                Price = book.Price
            }
        }
            };

            // Reduce the stock
            book.Stock -= orderDto.Quantity;

            _context.Orders.Add(newOrder);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Order placed successfully", orderId = newOrder.Id });
        }

        //// Using Stored Procedure
        //[HttpPost("PlaceOrderSP")]
        //public async Task<IActionResult> PlaceOrderUsingSP([FromBody] CreateOrderDto orderDto)
        //{
        //    try
        //    {
        //        var connection = _context.Database.GetDbConnection();
        //        await connection.OpenAsync();

        //        using (var command = connection.CreateCommand())
        //        {
        //            command.CommandText = "PlaceOrderFromCart";
        //            command.CommandType = System.Data.CommandType.StoredProcedure;

        //            var userIdParam = command.CreateParameter();
        //            userIdParam.ParameterName = "@UserId";
        //            userIdParam.Value = orderDto.UserId;
        //            command.Parameters.Add(userIdParam);

        //            var reader = await command.ExecuteReaderAsync();

        //            if (reader.Read())
        //            {
        //                int orderId = reader.GetInt32(reader.GetOrdinal("OrderId"));
        //                string message = reader.GetString(reader.GetOrdinal("Message"));

        //                return Ok(new { OrderId = orderId, Message = message });
        //            }

        //            return BadRequest("Order placement failed.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}

        //// Using Stored Procedure
        //[HttpPost("shop-now-sp")]
        //public async Task<IActionResult> ShopNowSP([FromBody] ShopNowOrderDto orderDto)
        //{
        //    if (orderDto == null || orderDto.UserId <= 0 || orderDto.BookId <= 0 || orderDto.Quantity <= 0)
        //        return BadRequest("Invalid order data.");

        //    try
        //    {
        //        var orderId = await _context.Database.ExecuteSqlRawAsync(
        //            "EXEC sp_ShopNowOrder @UserId = {0}, @BookId = {1}, @Quantity = {2}",
        //            orderDto.UserId, orderDto.BookId, orderDto.Quantity
        //        );

        //        return Ok(new { message = "Order placed successfully", orderId = orderId });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}



    }
}
