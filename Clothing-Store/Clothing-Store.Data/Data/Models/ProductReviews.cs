namespace Clothing_Store.Data.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class ProductReviews
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }

        public Product Product { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string UserFullName { get; set; } = null!;

        [Required]
        [MaxLength(250)]
        public string Message{ get; set; } = null!;

        public int Rating { get; set; }

        public DateTime Date { get; set; }

        public string? UserProfileImageUrl { get; set; }
    }
}
