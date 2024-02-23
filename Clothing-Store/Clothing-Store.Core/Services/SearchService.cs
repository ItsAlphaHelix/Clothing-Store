namespace Clothing_Store.Core.Services
{
    using Clothing_Store.Core.Contracts;
    using Clothing_Store.Core.ViewModels.Products;
    using Clothing_Store.Data.Data.Models;
    using Clothing_Store.Data.Repositories;
    using Microsoft.EntityFrameworkCore;
    public class SearchService : ISearchService
    {
        private readonly IRepository<Product> productsRepository;
        public SearchService(IRepository<Product> productsRepository)
        {
            this.productsRepository = productsRepository;
        }



        public IQueryable<ProductViewModel> SearchProductsByQueryAsQueryable(string query)
        {
            if (!string.IsNullOrWhiteSpace(query))
            {
                var baseForm = query.ToLower().Substring(0, Math.Min(4, query.Length));

                var searchTerm = $"%{baseForm}%";

                var searchProducts = this.productsRepository
                    .AllAsNoTracking()
                    .Where(x => EF.Functions.Like(x.Description.ToLower(), searchTerm))
                    .Select(x => new ProductViewModel()
                    {
                        Id = x.Id,
                        Category = x.Category,
                        Price = x.Price,
                        Images = x.Images.Select(i => i.Url).ToList()
                    })
                    .AsQueryable();

                return searchProducts;
            }

            return null;
        }
    }
}
