using Microsoft.EntityFrameworkCore;
using MyForeignCards.Entities;

namespace MyForeignCards.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Word> Words => Set<Word>();
        public DbSet<Category> Categories => Set<Category>();

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
    }
}
