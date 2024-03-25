namespace Clothing_Store.Core.Contracts
{
    using Clothing_Store.Core.ViewModels.Products;
    using Clothing_Store.Core.ViewModels.Reviews;
    using Clothing_Store.Core.ViewModels.Shared;

    public interface IProductsService
    {
        /// <summary>
        /// Getting all products.
        /// </summary>
        /// <returns></returns>
        public IQueryable<ProductViewModel> GetAllProductsAsQueryable(PaginatedViewModel<ProductViewModel> model);

        /// <summary>
        /// Getting current product by his id.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public Task<ProductViewModel> GetProductByIdAsync(int productId);

        /// <summary>
        /// Getting current product's information.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public Task<DetailsViewModel> GetProductDetailsByIdAsync(int productId, int pageNumber, int pageSize);

        /// <summary>
        /// Getting all products by gender.
        /// </summary>
        /// <returns></returns>
        public IQueryable<ProductViewModel> GetAllProductsByGenderAsQueryable(PaginatedViewModel<ProductViewModel> model, bool isMale);

        public Task<IEnumerable<SizeViewModel>> GetAllSizesAsync();

        public Task PostProductReviewAsync(PostProductReviewViewModel productReview, string userId);

        public Task<IEnumerable<ProductViewModel>> GetRecommendedProductsAsync(int productId);
    }
}
