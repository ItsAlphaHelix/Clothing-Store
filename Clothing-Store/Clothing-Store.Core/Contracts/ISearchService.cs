using Clothing_Store.Core.ViewModels.Products;

namespace Clothing_Store.Core.Contracts
{
    public interface ISearchService
    {
        public IQueryable<ProductViewModel> SearchProductsByQueryAsQueryable(string query);

        public IQueryable<ProductViewModel> FilterProductsAsQueryable(string[] queries);
    }
}
