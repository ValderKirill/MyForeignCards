using Microsoft.EntityFrameworkCore;
using MyForeignCards.Entities;

namespace MyForeignCards.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Word> Words => Set<Word>();
        public DbSet<Category> Categories => Set<Category>();

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Word>()
                .HasOne(w => w.Category)
                .WithMany(c => c.Words)
                .HasForeignKey(w => w.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
