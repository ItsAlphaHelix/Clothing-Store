using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clothing_Store.Data.Data.Models
{
    public class Image
    {
        [Key]
        public int Id { get; set; }

        public string Url { get; set; }

        [ForeignKey(nameof(Product))]
        public int ProductID { get; set; }
        public virtual Product Product { get; set; }
    }
}
