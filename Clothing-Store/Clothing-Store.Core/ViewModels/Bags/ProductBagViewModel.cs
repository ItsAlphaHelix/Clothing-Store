using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothing_Store.Core.ViewModels.Bags
{
    public class ProductBagViewModel
    {
        public int Id { get; set; }

        public string CategoryName { get; set; }

        public decimal Price { get; set; }

        public string SizeName { get; set; }

        public int Quantity { get; set; }

        public string ImageUrl { get; set; }

        public bool IsProductInStock { get; set; }
    }
}
