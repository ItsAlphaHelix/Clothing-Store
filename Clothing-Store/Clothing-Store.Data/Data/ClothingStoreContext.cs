namespace Clothing_Store.Data.Data
{
    using Clothing_Store.Data.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    public class ClothingStoreContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ClothingStoreContext(DbContextOptions<ClothingStoreContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<ProductReviews> ProductsReviews { get; set; }

        public DbSet<Size> Sizes { get; set; }

        public DbSet<ProductSize> ProductSizes { get; set; }

        public DbSet<Favorite> Favorites { get; set; }

        public DbSet<ProductFavorites> ProductFavorites { get; set; }

        public DbSet<Bag> Bags { get; set; }

        public DbSet<ProductBag> ProductBags { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<OrderProduct> OrderProducts { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ProductSize>()
                .HasKey(x => new { x.ProductId, x.SizeId });

            builder.Entity<ProductFavorites>()
                .HasKey(x => new { x.ProductId, x.FavoriteId });

            builder.Entity<ProductBag>()
                .HasKey(x => new { x.ProductId, x.BagId, x.SizeName });


            base.OnModelCreating(builder);
        }
    }
}
