using LearnAPI.AppDbContext;
using LearnAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearnAPI.Controllers
{
    [ApiController]
    // [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    public class BookController : Controller
    {
        private readonly BookNookDbContext _context;

        public BookController(BookNookDbContext context)
        {
            _context = context;
        }

        // get: api/book
        [HttpGet]
        [Route("index")]
        public async Task<IActionResult> GetBooks()
        {
            try
            {
                var books = await _context.Books.ToListAsync();
                return Ok(books); // Returns HTTP 200 with the list of books
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/book/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            try
            {
                var book = await _context.Books.FindAsync(id);
                if (book == null)
                    return NotFound($"Book with ID {id} not found.");

                return Ok(book);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        // POST: api/book
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<BookModel>> CreateBook([FromBody] BookModel bookModel)
        {
            if (bookModel == null)
                return BadRequest("Invalid book data.");
            try
            {
                _context.Books.Add(bookModel);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetBook), new { id = bookModel.Id }, bookModel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] BookModel bookModel)
        {
            if (bookModel == null)
                return BadRequest("Request body is missing.");

            if (id != bookModel.Id)
                return BadRequest("Book ID in the URL does not match the ID in the request body.");

            var book = await _context.Books.FindAsync(id);
            if (book == null)
                return NotFound($"No book found with ID {id}.");

            // Update book properties
            book.Title = bookModel.Title;
            book.Author = bookModel.Author;
            book.Price = bookModel.Price;
            // Map other properties as needed

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Books.Any(e => e.Id == id))
                    return NotFound($"Concurrency issue: Book with ID {id} was deleted or modified by another user.");

                return StatusCode(500, "An error occurred while updating the book. Please try again.");
            }

            return Ok(new { message = "Book updated successfully.", book });
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
                return NotFound($"No book found with ID {id}.");

            _context.Books.Remove(book);

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = $"Book with ID {id} deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting the book: {ex.Message}");
            }
        }

    }
}
