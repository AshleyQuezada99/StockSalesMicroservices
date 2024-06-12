using Microsoft.EntityFrameworkCore;
using Sale.Entities;


namespace Sale.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) { }

        public DbSet<Sales> Sales {  get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Sales>().Property(s => s.ProductId).IsRequired();
            modelBuilder.Entity<Sales>().Property(s => s.Amount).IsRequired();
            modelBuilder.Entity<Sales>().Property(s => s.DateSale).HasDefaultValueSql("GETDATE()");
            base.OnModelCreating(modelBuilder);
        }

    }
}
