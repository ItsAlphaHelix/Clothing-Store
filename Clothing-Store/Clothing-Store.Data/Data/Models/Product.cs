namespace Clothing_Store.Data.Data.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string? ClearInfo { get; set; }
        public bool IsMale { get; set; }
        public List<Image> Images { get; set; } = new List<Image>();
    }
}
