using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothing_Store.Core.ViewModels.Products
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        public string Category { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }
    }
}
