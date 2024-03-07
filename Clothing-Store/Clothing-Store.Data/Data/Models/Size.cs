namespace Clothing_Store.Data.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    public class Size
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public virtual ICollection<ProductSize> ProductSizes { get; set; } = new HashSet<ProductSize>();
    }
}
