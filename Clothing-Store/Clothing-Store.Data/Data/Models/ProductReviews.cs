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
        [StringLength(50)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string EmailAddress{ get; set; } = null!;

        [Required]
        [StringLength(250)]
        public string Message{ get; set; } = null!;

        public int Rating { get; set; }

        public DateTime Date { get; set; }
    }
}
