namespace Clothing_Store.Core.Services
{
    using Azure;
    using Clothing_Store.Core.Contracts;
    using Clothing_Store.Core.ViewModels;
    using Clothing_Store.Core.ViewModels.Products;
    using Clothing_Store.Data.Data.Models;
    using Clothing_Store.Data.Repositories;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> productsRepository;
        private readonly IRepository<ProductReviews> productReviewsRepository;
        private readonly UserManager<ApplicationUser> usersManager;
        private readonly IRepository<ApplicationUser> usersRepository;
        private readonly IRepository<Cart> cartsRepository;
        public ProductService(
            IRepository<Product> productsRepository,
            IRepository<ProductReviews> productReviewsRepository,
            UserManager<ApplicationUser> usersManager,
            IRepository<ApplicationUser> usersRepository,
            IRepository<Cart> cartsRepository)
        {
            this.productsRepository = productsRepository;
            this.productReviewsRepository = productReviewsRepository;
            this.usersManager = usersManager;
            this.usersRepository = usersRepository;
            this.cartsRepository = cartsRepository;
        }

        public IQueryable<ProductViewModel> GetlAllProductsByGenderAsQueryable(bool isMen)
        {
            var products = this.productsRepository
                .AllAsNoTracking()
                .Where(x => x.IsMale == isMen)
                .Select(x => new ProductViewModel()
                {
                    Id = x.Id,
                    Category = x.Category,
                    Price = x.Price,
                    Images = x.Images.Select(x => x.Url).Take(2).ToList()
                })
                .AsQueryable();

            return products;
        }

        public IQueryable<ProductViewModel> GetAllProductsAsQueryable()
        {
            var products = this.productsRepository
                .AllAsNoTracking()
                .Select(x => new ProductViewModel()
                {
                    Id = x.Id,
                    Category = x.Category,
                    Price = x.Price,
                    Images = x.Images.Select(x => x.Url).Take(2).ToList()
                })
                .AsQueryable();

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

        public async Task<ProductDetailsViewModel> GetProductDetailsByIdAsync(int productId, int pageNumber, int pageSize)
        {
            var reviews = await this.productReviewsRepository.AllAsNoTracking()
               .Where(x => x.ProductId == productId)
               .OrderByDescending(x => x.Date)
               .Skip((pageNumber - 1) * pageSize)
               .Take(pageSize)
               .Select(x => new GetProductReviewViewModel()
               {
                   ProductId = x.ProductId,
                   UserFullName = x.UserFullName,
                   Rating = x.Rating,
                   Message = x.Message,
                   Date = x.Date
               })
               .ToListAsync();

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
                    Reviews = reviews,
                    Images = x.Images.Select(x => x.Url)
                    .ToList()
                })
                .FirstOrDefaultAsync();

            return product;
        }

        public async Task PostProductReviewAsync(PostProductReviewViewModel productReview, string userId)
        {
            var user = await this.usersManager.FindByIdAsync(userId);

            var postProductReview = new ProductReviews()
            {
                ProductId = productReview.ProductId,
                UserFullName = user.FullName,
                Rating = productReview.Rating,
                Message = productReview.Message,
                Date = DateTime.Now
            };

            await this.productReviewsRepository.AddAsync(postProductReview);
            await this.productReviewsRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<GetProductReviewViewModel>> GetProductReviewsAsync(int productId)
        {
            var reviews = await this.productReviewsRepository.AllAsNoTracking()
                .Where(x => x.ProductId == productId)
                .Select(x => new GetProductReviewViewModel()
                {
                    ProductId = x.ProductId,
                    UserFullName = x.UserFullName,
                    Rating = x.Rating,
                    Message = x.Message,
                    Date = x.Date
                })
                .ToListAsync();

            return reviews;
        }
    }
}
