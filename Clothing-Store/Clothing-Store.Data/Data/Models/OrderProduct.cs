namespace Clothing_Store.Data.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class OrderProduct
    {
        [Key]
        public int Id { get; set; }


        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }

        public virtual Order Order { get; set; } = null!;

        public int ProductId { get; set; }

        [Required]
        public string CategoryName { get; set; } = null!;

        [Required]
        [Url]
        public string ImageUrl { get; set; } = null!;


        public decimal Price { get; set; }


        public decimal TotalPrice { get; set; }


        public int Quantity { get; set; }

        [Required]
        public string SizeName { get; set; } = null!;
    }
}
