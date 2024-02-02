namespace Clothing_Store.Data.Data.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Category { get; set; } = null!;

        public decimal Price { get; set; } 

        public string Description { get; set; } = null!;

        public string? ClearInfo { get; set; }

        public int AverageRating { get; set; }

        public bool IsMale { get; set; }

        public List<Image> Images { get; set; } = new List<Image>();
    }
}
