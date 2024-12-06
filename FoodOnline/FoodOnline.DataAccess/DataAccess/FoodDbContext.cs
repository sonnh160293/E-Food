using FoodOnline.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FoodOnline.DataAccess.DataAccess
{
    public class FoodDbContext : IdentityDbContext<ApplicationUser>
    {

        public FoodDbContext(DbContextOptions<FoodDbContext> options) : base(options) { }

        public DbSet<Branch> Branches { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductDetail> ProductDetails { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }

        public DbSet<ProductRelated> ProductRelateds { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderStatus> OrderStatus { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductRelated>()
           .HasOne(pr => pr.Product)
           .WithMany()
           .HasForeignKey(pr => pr.ProductId)
           .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductRelated>()
                .HasOne(pr => pr.RelatedProduct)
                .WithMany()
                .HasForeignKey(pr => pr.ProductRelatedId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
