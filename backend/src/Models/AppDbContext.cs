using Microsoft.EntityFrameworkCore;

namespace BackendApi.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        
        public DbSet<Product> Products => Set<Product>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
                entity.Property(e => e.Description).HasColumnType("text");
                entity.Property(e => e.Category).HasMaxLength(100);
                entity.Property(e => e.Brand).HasMaxLength(100);
                entity.Property(e => e.Price).HasColumnType("decimal(15,2)");
                entity.Property(e => e.SKU).HasMaxLength(100).IsRequired();
                entity.Property(e => e.ReleaseDate).HasColumnType("date");
                entity.Property(e => e.AvailabilityStatus).HasMaxLength(50);
                entity.Property(e => e.CustomerRating).HasColumnType("decimal(3,2)");
                entity.Property(e => e.AvailableColors).HasMaxLength(500);
                entity.Property(e => e.AvailableSizes).HasMaxLength(500);
                
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
