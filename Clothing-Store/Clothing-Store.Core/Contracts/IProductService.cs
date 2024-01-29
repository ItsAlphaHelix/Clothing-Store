namespace Clothing_Store.Core.Contracts
{
    using Clothing_Store.Core.ViewModels.Products;
    public interface IProductService
    {
        /// <summary>
        /// Getting all products.
        /// </summary>
        /// <returns></returns>
        public Task<ICollection<ProductViewModel>> GetAllProductsAsync();
    }
}
