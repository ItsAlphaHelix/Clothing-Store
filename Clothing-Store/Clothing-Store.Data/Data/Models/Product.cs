namespace Clothing_Store.Data.Data.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Category { get; set; } = null!;

        public decimal Price { get; set; } 

        public string Description { get; set; } = null!;

        public string? ClearInfo { get; set; }

        public bool IsMale { get; set; }

        public virtual ICollection<Image> Images { get; set; } = new HashSet<Image>();

        public virtual ICollection<ProductReviews> ProductReviews { get; set; } = new HashSet<ProductReviews>();

        public virtual ICollection<ProductSize> ProductSizes { get; set; } = new HashSet<ProductSize>();
    }
}
