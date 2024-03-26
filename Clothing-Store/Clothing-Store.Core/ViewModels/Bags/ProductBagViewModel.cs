using Clothing_Store.Core.ViewModels.Products;

namespace Clothing_Store.Core.ViewModels.Bags
{
    public class ProductBagViewModel
    {
        public int Id { get; set; }

        public string CategoryName { get; set; }

        public decimal Price { get; set; }

        public string SizeName { get; set; }

        public int Quantity { get; set; }

        public string ImageUrl { get; set; }

        public bool IsProductInStock { get; set; }

        public bool IsDeleted { get; set; }
    }
}
