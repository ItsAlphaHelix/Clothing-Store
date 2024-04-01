using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clothing_Store.Core.ViewModels.Customers;

namespace Clothing_Store.Core.ViewModels.Orders
{
    public class MineOrdersViewModel
    {
        public IQueryable<OrderViewModel> OrdersModel { get; set; }
    }
}
