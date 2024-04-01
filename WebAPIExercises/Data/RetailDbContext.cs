using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using WebAPIExercises.Models;

namespace WebAPIExercises.Data
{
    public class RetailDbContext : DbContext
    {
        public RetailDbContext(DbContextOptions<RetailDbContext> options): base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Store>()
                    .HasMany(e => e.Products)
                    .WithMany(e => e.Stores)
                    .UsingEntity<Inventory>();
        }

    }
}
