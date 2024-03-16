namespace Clothing_Store.Data.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string OrderNumber { get; set; } = null!;

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        [ForeignKey(nameof(Customer))]
        public string CustomerId { get; set; } = null!;

        public virtual Customer Customer { get; set; } = null!;

        public string? StripePaymentStatus { get; set; }

        public string? StripePaymentIntendId { get; set; }

        public string? StripeSessionId { get; set; }
        public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new HashSet<OrderProduct>();
    }
}