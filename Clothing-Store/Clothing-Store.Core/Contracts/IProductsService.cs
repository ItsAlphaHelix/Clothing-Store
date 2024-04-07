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
        IQueryable<ProductViewModel> GetAllProductsAsQueryable(PaginatedViewModel<ProductViewModel> model);

        /// <summary>
        /// Getting current product by his id.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<ProductViewModel> GetProductByIdAsync(int productId);

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
        IQueryable<ProductViewModel> GetAllProductsByGenderAsQueryable(PaginatedViewModel<ProductViewModel> model, bool isMale);

        Task<IEnumerable<SizeViewModel>> GetAllSizesAsync();

        Task PostProductReviewAsync(PostProductReviewViewModel productReview, string userId);

        Task<IEnumerable<ProductViewModel>> GetRecommendedProductsAsync(int productId);
    }
}
