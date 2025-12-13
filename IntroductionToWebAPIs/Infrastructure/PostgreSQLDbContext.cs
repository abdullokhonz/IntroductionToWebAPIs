using IntroductionToWebAPIs.BaseEntities;
using IntroductionToWebAPIs.Entity;
using IntroductionToWebAPIs.Entity.Users;
using Microsoft.EntityFrameworkCore;

namespace IntroductionToWebAPIs.Infrastructure
{
    public class PostgreSQLDbContext : DbContext
    {
        public PostgreSQLDbContext(DbContextOptions<PostgreSQLDbContext> options) : base(options)
        {

        }

        public DbSet<Units> Units { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Price> Prices { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Branch> Branches { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<BaseEntity>();

            // FluentAPI
            modelBuilder.Entity<Units>(entity => { entity.HasKey(p => p.Id); });
            modelBuilder.Entity<User>(entity => { entity.HasKey(p => p.Id); });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(p => p.IsPersonalDataAccepted)
                .IsRequired()
                .HasDefaultValue(false);
            });

            modelBuilder.Entity<User>()
                .HasOne(u => u.Profile)
                .WithOne(p => p.User)
                .HasForeignKey<UserProfile>(p => p.UserId);

            modelBuilder.Entity<Supplier>(entity => { entity.HasKey(p => p.Id); });
            modelBuilder.Entity<Category>(b =>
            {
                b.HasKey(p => p.Id);
                b.HasOne(p => p.Parent)
                .WithMany(p => p.Children)
                .HasForeignKey(p => p.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<Warehouse>(entity => { entity.HasKey(p => p.Id); });
            modelBuilder.Entity<Product>(entity => { entity.HasKey(p => p.Id); });
            modelBuilder.Entity<Price>(b =>
            {
                b.HasKey(p => p.Id);
                b.HasOne(p => p.Product)
                 .WithMany(p => p.Prices)
                 .HasForeignKey(p => p.ProductId)
                 .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Position>(b =>
            {
                b.HasKey(p => p.Id);
                b.HasOne(p => p.Parent)
                 .WithMany(p => p.Children)
                 .HasForeignKey(p => p.ParentId)
                 .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<Branch>(b =>
            {
                b.HasKey(p => p.Id);
                b.HasOne(p => p.Parent)
                 .WithMany(p => p.Children)
                 .HasForeignKey(p => p.ParentId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
