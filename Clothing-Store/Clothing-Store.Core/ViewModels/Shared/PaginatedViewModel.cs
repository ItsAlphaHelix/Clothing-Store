namespace Clothing_Store.Core.ViewModels.Shared
{
    using Clothing_Store.Core.ViewModels.Orders;
    using Clothing_Store.Core.ViewModels.Products;
    public class PaginatedViewModel<T>
    {
        public PaginatedList<T> Models { get; set; }

        public CustomerViewModel CustomerModel { get; set; }

        public SortEnum Sorting { get; set; }

        public string SelectedProducts { get; set; }

        public string SelectedPrice { get; set; }

        public string SelectedSizes { get; set; }

        public IEnumerable<ProductViewModel> RecommendedProducts { get; set; } = new List<ProductViewModel>();
    }
}
