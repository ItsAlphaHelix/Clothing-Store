namespace Clothing_Store.Data.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    public class Bag
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; } = null!;

        public virtual ICollection<ProductBag> ProductBags { get; set; } = new HashSet<ProductBag>();
    }
}
