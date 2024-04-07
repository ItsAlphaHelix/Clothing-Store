namespace Clothing_Store.Core.Contracts
{
    using Clothing_Store.Core.ViewModels.Products;
    public interface IFavoritesService
    {
        IQueryable<ProductViewModel> AllFavoritesProductsAsQueryable(string userId);

        Task AddFavoriteProduct(string userId, int productId);

        Task<int> CountOfFavoriteProductsAsync(string userId);

        Task<IEnumerable<ProductViewModel>> AllUserFavoriteProductsAsync(string userId);

        Task DeleteFavoriteProduct(int productId);
    }
}
