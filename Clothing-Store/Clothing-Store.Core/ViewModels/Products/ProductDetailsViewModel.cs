namespace Clothing_Store.Core.ViewModels.Products
{
    public class ProductDetailsViewModel
    {
        public int Id { get; set; }
        public string Category { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string ClearInfo { get; set; }

        public bool IsMale { get; set; }

        public List<string> Images { get; set; } = new List<string>();
    }
}
