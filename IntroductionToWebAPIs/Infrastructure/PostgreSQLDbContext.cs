using IntroductionToWebAPIs.BaseEntities;
using IntroductionToWebAPIs.Entity;
using Microsoft.EntityFrameworkCore;

namespace IntroductionToWebAPIs.Infrastructure
{
    public class PostgreSQLDbContext : DbContext
    {
        public PostgreSQLDbContext(DbContextOptions<PostgreSQLDbContext> options) : base(options)
        {

        }

        public DbSet<Unit> Units { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Price> Prices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<BaseEntity>();

            // FluentAPI
            modelBuilder.Entity<Unit>(entity => { entity.HasKey(p => p.Id); });
            modelBuilder.Entity<User>(entity => { entity.HasKey(p => p.Id); });
            modelBuilder.Entity<Supplier>(entity => { entity.HasKey(p => p.Id); });
            modelBuilder.Entity<Category>()
                .HasOne(c => c.Parent)
                .WithMany(c => c.Children)
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Warehouse>(entity => { entity.HasKey(p => p.Id); });
            modelBuilder.Entity<Product>(entity => { entity.HasKey(p => p.Id); });
            modelBuilder.Entity<Price>()
                .HasOne(p => p.Product)
                .WithMany(p => p.Prices)
                .HasForeignKey(p => p.ProductId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
