using Microsoft.EntityFrameworkCore;
using MyForeignCards.Entities;

namespace MyForeignCards.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Word> Words => Set<Word>();
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Word>().HasData(
                new Word { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Text = "Apple", Translation = "Яблоко" },
                new Word { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Text = "Table", Translation = "Стол" });
        }
    }
}
