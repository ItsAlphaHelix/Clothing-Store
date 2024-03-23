namespace Clothing_Store.Core.Contracts
{
    using Clothing_Store.Core.ViewModels.Bags;

    public interface IBagsService
    {
        public IQueryable<ProductBagViewModel> GetAllProductsInBagAsQueryable(string userId);

        public Task AddProductToBagAsync(int productId, string sizeName, int quantity, string userId);

        public Task<decimal> CalculateTotalPrice(string userId);

        public Task<int> CountOfProductsInBagAsync(string userId);

        string GetOrCreateTemporaryUserId();

        public Task DeleteProductFromBagAsync(int? productId, string userId = null);

        public Task<int> GetTotalQuantityOfSizeOfProduct(string sizeName, int productId);

        public Task DecrementQuantityOfProductAsync(string sizeName, int productId, string userId);

        public Task IncrementQuantityOfProductAsync(string sizeName, int productId, string userId, int currentQuantity);
    }
}
