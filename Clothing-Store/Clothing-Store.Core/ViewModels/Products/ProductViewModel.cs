using Clothing_Store.Data.Data.Models;

namespace Clothing_Store.Core.ViewModels.Products
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        public string Category { get; set; }

        public decimal Price { get; set; }

        public double AverageRating { get; set; }

        public List<string> Images { get; set; } = new List<string>();

        public bool IsProductInStock { get; set; }

        public IEnumerable<SizeViewModel> ProductSizes { get; set; } = new List<SizeViewModel>();
    }
}
