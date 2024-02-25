using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothing_Store.Data.Data.Models
{
    public class ProductSize
    {
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }

        public virtual Product Product { get; set; } = null!;

        [ForeignKey(nameof(Size))]
        public int SizeId { get; set; }

        public virtual Size Size { get; set; } = null!;

        public int Count { get; set; }
    }
}
