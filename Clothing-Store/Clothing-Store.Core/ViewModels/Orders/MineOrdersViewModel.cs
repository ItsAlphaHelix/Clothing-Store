using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothing_Store.Core.ViewModels.Orders
{
    public class MineOrdersViewModel
    {
        public IQueryable<OrderViewModel> OrdersModel { get; set; }

        public CustomerViewModel CustomerModel { get; set; }
    }
}
