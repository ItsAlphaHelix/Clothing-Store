namespace Clothing_Store.Data.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    public class Bag
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = null!;

        public DateTime? DeletedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public DateTime? CreatedOn { get; set; }

        public virtual ICollection<ProductBag> ProductBags { get; set; } = new HashSet<ProductBag>();
    }
}
