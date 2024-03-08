using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothing_Store.Core.ViewModels.Orders
{
    public class OrderViewModel
    {
        public string NumberOfOrder { get; set; }

        public string OrderDate { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
