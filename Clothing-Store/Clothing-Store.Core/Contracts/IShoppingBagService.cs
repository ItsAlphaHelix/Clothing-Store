namespace Clothing_Store.Core.Contracts
{
    using Clothing_Store.Core.ViewModels.Bags;
    using Clothing_Store.Core.ViewModels.Products;

    public interface IShoppingBagService
    {
        public Task<IEnumerable<BagViewModel>> GetAllProductsInBagAsync(string userId);

        public Task AddProductToBag(int productId, string sizeName, int quantity, string userId);

        public Task<decimal> CalculateTotalPrice(string userId);

        public Task<int> CountOfProductsInBagAsync(string userId);

        string GetOrCreateTemporaryUserId();

        public Task DeleteProductFromBagAsync(int bagId);
    }
}
