namespace Clothing_Store.Data.Data
{
    using Clothing_Store.Data.Data.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    public class ClothingStoreContext : IdentityDbContext<ApplicationUser>
    {
        public ClothingStoreContext(DbContextOptions<ClothingStoreContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<ProductReviews> ProductsReviews { get; set; }

        public DbSet<Cart> Carts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
