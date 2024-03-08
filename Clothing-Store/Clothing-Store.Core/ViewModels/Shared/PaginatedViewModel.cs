using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clothing_Store.Core.ViewModels.Orders;
using Clothing_Store.Core.ViewModels.Products;

namespace Clothing_Store.Core.ViewModels.Shared
{
    public class PaginatedViewModel<T>
    {
        public PaginatedList<T> Models { get; set; }

        public CustomerViewModel CustomerModel { get; set; }

        public SortEnum Sorting { get; set; }

        public string SelectedProducts { get; set; }

        public string SelectedPrice { get; set; }

        public string SelectedSizes { get; set; }
    }
}
