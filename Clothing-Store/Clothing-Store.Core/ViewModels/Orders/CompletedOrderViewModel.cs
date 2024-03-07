using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothing_Store.Core.ViewModels.Orders
{
    public class CompletedOrderViewModel : CustomerViewModel
    {
        public string OrderDate { get; set; }

        public string OrderNumber { get; set; }

        public IEnumerable<ProductOrderViewModel> ProductOrderModel { get; set; } = new List<ProductOrderViewModel>();
    }
}
