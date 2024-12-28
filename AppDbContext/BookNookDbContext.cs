using LearnAPI.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace LearnAPI.AppDbContext
{
    public class BookNookDbContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }

        public BookNookDbContext(DbContextOptions<BookNookDbContext> options): base(options)
        {
        }
    }
}
