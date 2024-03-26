namespace Clothing_Store.Core.Services
{
    using Clothing_Store.Core.Contracts;
    using Clothing_Store.Core.Services.HelperServices;
    using Clothing_Store.Core.ViewModels.Products;
    using Clothing_Store.Core.ViewModels.Shared;
    using Clothing_Store.Data.Data.Models;
    using Clothing_Store.Data.Repositories;
    using Microsoft.EntityFrameworkCore;
    public class SearchService : FilterHelperService, ISearchService
    {
        private readonly IRepository<Product> productsRepository;
        public SearchService(IRepository<Product> productsRepository)
        {
            this.productsRepository = productsRepository;
        }



        public IQueryable<ProductViewModel> SearchProductsByQueryAsQueryable(PaginatedViewModel<ProductViewModel> model, string searchBy)
        {
            if (!string.IsNullOrWhiteSpace(searchBy))
            {
                var baseForm = searchBy.ToLower().Substring(0, Math.Min(4, searchBy.Length));

                var searchTerm = $"%{baseForm}%";

                var searchProducts = this.productsRepository
                    .AllAsNoTracking()
                    .Where(x => EF.Functions.Like(x.Description.ToLower(), searchTerm))
                    .Select(x => new ProductViewModel()
                    {
                        Id = x.Id,
                        Category = x.Category,
                        Price = x.Price,
                        AverageRating = x.ProductReviews.Any() ? (x.ProductReviews.Sum(x => x.Rating) / x.ProductReviews.Count) : 0,
                        Images = x.Images.Select(x => x.Url).Take(2).ToList(),
                        ProductSizes = x.ProductSizes
                        .Where(x => x.Count != 0)
                        .Select(x => x.Size.Name)
                        .ToList()
                    })
                    .AsQueryable();

                searchProducts = FilterProducts(model, searchProducts);

                return searchProducts;
            }

            return null;
        }
    }
}
