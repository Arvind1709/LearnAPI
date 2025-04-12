using LearnAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace LearnAPI.AppDbContext
{
    public class BookNookDbContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }
        public DbSet<BookModel> Books { get; set; }
        public DbSet<CartModel> Carts { get; set; }
        public DbSet<CartItemModel> CartItems { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<OrderItemModel> OrderItems { get; set; }

        public BookNookDbContext(DbContextOptions<BookNookDbContext> options) : base(options)
        {
        }
    }
}
