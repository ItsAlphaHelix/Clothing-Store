using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothing_Store.Core.ViewModels.Products
{
    public class PaginatedViewModel
    {
        public PaginatedList<ProductViewModel> Products { get; set; }

        public SortEnum Sorting { get; set; }

        public string SelectedProducts { get; set; }

        public string SelectedPrice { get; set; }

        public string SelectedSizes { get; set; }
    }
}
