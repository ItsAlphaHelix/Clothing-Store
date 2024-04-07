using Clothing_Store.Core.ViewModels.Products;
using Clothing_Store.Core.ViewModels.Shared;

namespace Clothing_Store.Core.Contracts
{
    public interface ISearchService
    {
        IQueryable<ProductViewModel> SearchProductsByQueryAsQueryable(PaginatedViewModel<ProductViewModel> model, string searchBy);
    }
}
