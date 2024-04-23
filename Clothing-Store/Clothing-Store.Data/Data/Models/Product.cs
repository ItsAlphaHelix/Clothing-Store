using System.ComponentModel.DataAnnotations;

namespace Clothing_Store.Data.Data.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string LCProductId { get; set; }

        [Required]
        public string LCProductColorId { get; set; }

        [Required]
        public string Category { get; set; } = null!;

        public decimal Price { get; set; }

        [Required]
        public string Description { get; set; } = null!;

        public string? ClearInfo { get; set; }

        public bool IsMale { get; set; }

        public virtual ICollection<Image> Images { get; set; } = new HashSet<Image>();

        public virtual ICollection<ProductReviews> ProductReviews { get; set; } = new HashSet<ProductReviews>();

        public virtual ICollection<ProductSize> ProductSizes { get; set; } = new HashSet<ProductSize>();

        public virtual ICollection<ProductFavorites> ProductFavorites { get; set; } = new HashSet<ProductFavorites>();

        public virtual ICollection<ProductBag> ProductBags { get; set; } = new HashSet<ProductBag>();
    }
}
