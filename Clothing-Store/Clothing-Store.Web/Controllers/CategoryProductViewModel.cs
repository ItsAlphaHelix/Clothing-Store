namespace Clothing_Store.Controllers
{
    public class CategoryProductViewModel
    {
        public List<Category> Categories { get; set; }

        public List<Product> Products { get; set; }

        public int TotalPages { get; set; }

        public int CurrentPage { get; set; }

        public bool HasPreviousPage => this.CurrentPage > 1;
        public bool HasNextPage => this.CurrentPage < this.TotalPages;
    }
}
