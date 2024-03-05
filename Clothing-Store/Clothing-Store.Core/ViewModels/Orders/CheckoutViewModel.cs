using Clothing_Store.Core.ViewModels.Bags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothing_Store.Core.ViewModels.Orders
{
    public  class CheckoutViewModel
    {
        public IEnumerable<BagViewModel> ProductsInBag { get; set; } = new List<BagViewModel>();
    }
}
