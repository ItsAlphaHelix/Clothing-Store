namespace Clothing_Store.Core.ViewModels.Products
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        public string Category { get; set; }

        public decimal Price { get; set; }

        public double AverageRating { get; set; }


        public bool IsProductInStock { get; set; }

        public bool IsDeleted { get; set; }

        public List<string> Images { get; set; } = new List<string>();

        public List<string> ProductSizes { get; set; } = new List<string>();
    }
}
