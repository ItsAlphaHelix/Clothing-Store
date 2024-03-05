using Clothing_Store.Core.ViewModels.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothing_Store.Core.Contracts
{
    public interface IOrderService
    {
        public Task AddOrderAsync(OrderViewModel orderModel);
    }
}
