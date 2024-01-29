namespace Clothing_Store.Core.Services
{
    using Clothing_Store.Core.Contracts;
    using Clothing_Store.Core.ViewModels.Products;
    using Clothing_Store.Data.Data.Models;
    using Clothing_Store.Data.Repositories;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> productsRepository;
        public ProductService(IRepository<Product> productsRepository)
        {
            this.productsRepository = productsRepository;

        }
        public async Task<ICollection<ProductViewModel>> GetAllProductsAsync()
        {
            var products = await this.productsRepository
                .AllAsNoTracking()
                .Select(x => new ProductViewModel()
                {
                    Id = x.Id,
                    Category = x.Category,
                    Price = x.Price,
                    ImageUrl = x.Images.FirstOrDefault().Url
                })
                .ToListAsync();

            return products;
        }
    }
}
