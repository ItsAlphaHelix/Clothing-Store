using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothing_Store.Core.ViewModels.Orders
{
    public class ProductOrderViewModel
    {
        public int Id { get; set; }

        public string CategoryName { get; set; }

        public string ImageUrl { get; set; }

        public int Quantity { get; set; }

        public decimal Price  { get; set; }

        public string SizeName  { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
