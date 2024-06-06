using Microsoft.EntityFrameworkCore;
using Stock.Entities;

namespace Stock.Data
{
    public class ApplicationDbContextStock : DbContext
    {
        public ApplicationDbContextStock(DbContextOptions options) : base(options) { }

        public DbSet<Products> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Products>().Property(p => p.Name).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Products>().Property(p => p.Price).IsRequired().HasColumnType("decimal");
            modelBuilder.Entity<Products>().Property(p => p.Stock).HasDefaultValue(0);
            modelBuilder.Entity<Products>().Property(p => p.Status).HasDefaultValue(true);
            base.OnModelCreating(modelBuilder);
        }
    }
}
