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
        private readonly IRepository<ProductReviews> productReviewsRepository;
        public ProductService(
            IRepository<Product> productsRepository,
            IRepository<ProductReviews> productReviewsRepository)
        {
            this.productsRepository = productsRepository;
            this.productReviewsRepository = productReviewsRepository;
        }

        public async Task<ICollection<ProductViewModel>> GetlAllProductsByGenderAsync(bool isMen)
        {
            var products = await this.productsRepository
                .AllAsNoTracking()
                .Where(x => x.IsMale == isMen)
                .Select(x => new ProductViewModel()
                {
                    Id = x.Id,
                    Category = x.Category,
                    Price = x.Price,
                    Images = x.Images.Select(x => x.Url).Take(2).ToList()
                })
                .ToListAsync();

            return products;
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
                    Images = x.Images.Select(x => x.Url).Take(2).ToList()
                })
                .ToListAsync();

            return products;
        }

        public async Task<ProductViewModel> GetProductByIdAsync(int productId)
        {
            var product = await this.productsRepository
                .AllAsNoTracking()
                .Where(x => x.Id == productId)
                .Select(x => new ProductViewModel()
                {
                    Id = x.Id,
                    Category = x.Category,
                    Price = x.Price,
                    Images = x.Images.Select(x => x.Url)
                    .ToList()
                })
                .FirstOrDefaultAsync();

            return product;
        }

        public async Task<ProductDetailsViewModel> GetProductDetailsByIdAsync(int productId)
        {
            var product = await this.productsRepository
                .AllAsNoTracking()
                .Where(x => x.Id == productId)
                .Select(x => new ProductDetailsViewModel()
                {
                    Id = x.Id,
                    Category = x.Category,
                    Price = x.Price,
                    Description = x.Description,
                    ClearInfo = x.ClearInfo,
                    IsMale = x.IsMale,
                    Images = x.Images.Select(x => x.Url)
                    .ToList()
                })
                .FirstOrDefaultAsync();

            return product;
        }

        public async Task PostProductReviewAsync(PostProductReviewViewModel productReview)
        {
            var postProductReview = new ProductReviews()
            {
                ProductId = productReview.ProductId,
                Name = productReview.Username,
                EmailAddress = productReview.EmailAddress,
                Rating = productReview.Rating,
                Message = productReview.Message,
                Date = DateTime.Now
            };

            await this.productReviewsRepository.AddAsync(postProductReview);
            await this.productReviewsRepository.SaveChangesAsync();
        }

        public async Task<ICollection<GetProductReviewViewModel>> GetReviewsForProductAsync(int productId)
        {
            var reviews = await this.productReviewsRepository.AllAsNoTracking()
                .Where(x => x.ProductId == productId)
                .Select(x => new GetProductReviewViewModel()
                {
                    ProductId = x.ProductId,
                    Username = x.Name,
                    Rating = x.Rating,
                    Message = x.Message,
                    Date = x.Date
                })
                .ToListAsync();

            return reviews;
        }
    }
}
