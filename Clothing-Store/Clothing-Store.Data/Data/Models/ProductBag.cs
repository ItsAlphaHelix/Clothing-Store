namespace Clothing_Store.Data.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class ProductBag
    {
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }

        public virtual Product Product { get; set; } = null!;


        [ForeignKey(nameof(Bag))]
        public int BagId { get; set; }

        public virtual Bag Bag { get; set; } = null!;

        [Required]
        public string SizeName { get; set; } = null!;

        public int Quantity { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public DateTime? CreatedOn { get; set; }
    }
}