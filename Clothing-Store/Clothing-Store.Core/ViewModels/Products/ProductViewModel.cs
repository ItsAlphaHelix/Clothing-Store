namespace Clothing_Store.Core.ViewModels.Products
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        public string Category { get; set; }

        public decimal Price { get; set; }

        public int AverageRating { get; set; }
        public List<string> Images { get; set; } = new List<string>();
    }
}
