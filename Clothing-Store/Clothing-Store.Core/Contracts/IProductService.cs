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
        public Task<ProductDetailsViewModel> GetProductDetailsByIdAsync(int productId);

        /// <summary>
        /// Getting all products by gender.
        /// </summary>
        /// <returns></returns>
        public Task<ICollection<ProductViewModel>> GetlAllProductsByGenderAsync(bool isMen);

        public Task PostProductReviewAsync(PostProductReviewViewModel productReview, string userId);
        //public Task PostProductReviewAsync(PostProductReviewViewModel productReview);

        public Task<ICollection<GetProductReviewViewModel>> GetReviewsForProductAsync(int productId);
    }
}
