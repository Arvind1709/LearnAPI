using LearnAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace LearnAPI.AppDbContext
{
    public class BookNookDbContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }
        public DbSet<BookModel> Books { get; set; }

        public BookNookDbContext(DbContextOptions<BookNookDbContext> options) : base(options)
        {
        }
    }
}
