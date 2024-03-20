namespace Clothing_Store.Core.Contracts
{
    using Clothing_Store.Core.ViewModels.Products;
    public interface IFavoritesService
    {
        public IQueryable<ProductViewModel> AllFavoritesProductsAsQueryable(string userId);

        public Task AddFavoriteProduct(string userId, int productId);

        public Task<int> CountOfFavoriteProductsAsync(string userId);

        public Task<IEnumerable<ProductViewModel>> AllUserFavoriteProductsAsync(string userId);

        public Task DeleteFavoriteProduct(int productId);
    }
}
