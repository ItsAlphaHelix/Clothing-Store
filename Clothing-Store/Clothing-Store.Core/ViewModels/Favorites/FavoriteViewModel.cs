using Clothing_Store.Core.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothing_Store.Core.ViewModels.Favorites
{
    public class FavoriteViewModel
    {
        public int Id { get; set; }

        public bool IsProductInStock { get; set; }

        public IEnumerable<ProductViewModel> FavoriteProducts { get; set; } = new List<ProductViewModel>();
    }
}
