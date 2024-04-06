namespace Clothing_Store.Core.Services
{
    using Clothing_Store.Core.Contracts;
    using Clothing_Store.Core.Services.HelperServices;
    using Clothing_Store.Core.ViewModels.Products;
    using Clothing_Store.Core.ViewModels.Reviews;
    using Clothing_Store.Core.ViewModels.Shared;
    using Clothing_Store.Data.Data.Models;
    using Clothing_Store.Data.Repositories;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProductsService : FilterHelperService, IProductsService
    {
        private readonly IRepository<Product> productsRepository;
        private readonly IRepository<ProductReviews> productReviewsRepository;
        private readonly IRepository<Size> sizesRepository;
        private readonly UserManager<ApplicationUser> usersManager;
        public ProductsService(
            IRepository<Product> productsRepository,
            IRepository<ProductReviews> productReviewsRepository,
            IRepository<Size> sizesRepository,
            UserManager<ApplicationUser> usersManager)
        {
            this.productsRepository = productsRepository;
            this.productReviewsRepository = productReviewsRepository;
            this.sizesRepository = sizesRepository;
            this.usersManager = usersManager;
        }

        public IQueryable<ProductViewModel> GetAllProductsByGenderAsQueryable(PaginatedViewModel<ProductViewModel> model, bool isMale)
        {
            var products = this.productsRepository
                .AllAsNoTracking()
                .Where(x => x.IsMale == isMale)
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

            products = FilterProducts(model, products);

            return products;
        }

        public IQueryable<ProductViewModel> GetAllProductsAsQueryable(PaginatedViewModel<ProductViewModel> model)
        {
            var products = this.productsRepository
                .AllAsNoTracking()
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

            products = FilterProducts(model, products);

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
                    IsProductInStock = x.ProductSizes.Any(x => x.Count != 0),
                    ProductSizes = x.ProductSizes
                    .Where(x => x.Count != 0)
                    .Select(x => x.Size.Name)
                    .ToList(),
                    Images = x.Images.Select(x => x.Url)
                    .ToList(),
                    
                })
                .FirstOrDefaultAsync();

            return product;
        }

        public async Task<DetailsViewModel> GetProductDetailsByIdAsync(int productId, int pageNumber, int pageSize)
        {
            int countOfReviews = await this.GetProductReviewsCountAsync(productId);
            double averageRatingOfProduct = await this.CalculateAverageOfCurrentProduct(productId);
            
            var product = await this.productsRepository
                .AllAsNoTracking()
                .Where(x => x.Id == productId)
                .Select(x => new DetailsViewModel()
                {
                    Id = x.Id,
                    Category = x.Category,
                    Price = x.Price,
                    Description = x.Description,
                    ClearInfo = x.ClearInfo,
                    IsMale = x.IsMale,
                    CountOfReviews = countOfReviews,
                    FiveStarts = x.ProductReviews.Where(x => x.Rating == 5.0).Count() != 100 ? x.ProductReviews.Where(x => x.Rating == 5.0).Count() : 100,
                    FourStarts = x.ProductReviews.Where(x => x.Rating == 4.0).Count() != 100 ? x.ProductReviews.Where(x => x.Rating == 4.0).Count() : 100,
                    ThreeStars = x.ProductReviews.Where(x => x.Rating == 3.0).Count() != 100 ? x.ProductReviews.Where(x => x.Rating == 3.0).Count() : 100,
                    TwoStars = x.ProductReviews.Where(x => x.Rating == 2.0).Count() != 100 ? x.ProductReviews.Where(x => x.Rating == 2.0).Count() : 100,
                    OneStar = x.ProductReviews.Where(x => x.Rating == 1.0).Count() != 100 ? x.ProductReviews.Where(x => x.Rating == 1.0).Count() : 100,
                    AverageRating = averageRatingOfProduct,
                    PercentageOfAverageStars = double.Parse(averageRatingOfProduct.ToString("F1")) *  20,
                    IsProductInStock = x.ProductSizes.Any(x => x.Count != 0),
                    Reviews = x.ProductReviews
                    .OrderByDescending(x => x.Date)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(x => new GetProductReviewViewModel() {
                        ProductId = x.ProductId,
                        UserFullName = x.UserFullName,
                        Rating = x.Rating,
                        Message = x.Message,
                        Date = x.Date,
                        UserProfileImageUrl = x.UserProfileImageUrl
                    }),
                    Sizes = x.ProductSizes
                    .Where(x => x.Count != 0)
                    .Select(x => new SizeViewModel()
                    {
                        SizeName = x.Size.Name
                    }),
                    Images = x.Images.Select(x => x.Url)
                    .ToList()
                })
                .FirstOrDefaultAsync();

            return product;
        }

        public async Task PostProductReviewAsync(PostProductReviewViewModel productReview, string userId)
        {
            var user = await this.usersManager.FindByIdAsync(userId);
            string userFullName = $"{user.FirstName} {user.LastName}";

            var review = new ProductReviews()
            {
                ProductId = productReview.ProductId,
                UserFullName = userFullName,
                Rating = productReview.Rating,
                Message = productReview.Message,
                Date = DateTime.Now,
                UserProfileImageUrl = user.ProfileImageUrl,
            };

            var product = await this.productsRepository
                .All()
                .Include(x => x.ProductReviews)
                .FirstOrDefaultAsync(x => x.Id == productReview.ProductId);
            product.ProductReviews.Add(review);
            await this.productReviewsRepository.SaveChangesAsync();
        }


        public async Task<IEnumerable<SizeViewModel>> GetAllSizesAsync()
        {
            var sizes = await this.sizesRepository
                .AllAsNoTracking()
                .Select(x => new SizeViewModel()
                {
                    SizeName = x.Name
                })
                .ToListAsync();

            return sizes;
        }

        private async Task<double> CalculateAverageOfCurrentProduct(int productId)
        {
            var productReviews = await this.productReviewsRepository
                .AllAsNoTracking()
                .Where(x => x.ProductId == productId)
                .ToListAsync();

            double averageRatingOfProduct = productReviews.Any() ? (productReviews.Sum(x => x.Rating) / productReviews.Count) : 0;

            return averageRatingOfProduct;
        }
        private async Task<int> GetProductReviewsCountAsync(int productId)
        {
            var countOfReviews = await this.productReviewsRepository.AllAsNoTracking()
                .Where(x => x.ProductId == productId)
                .CountAsync();

            return countOfReviews;
        }

        public async Task<IEnumerable<ProductViewModel>> GetRecommendedProductsAsync(int productId)
        {
            var product = await this.GetProductDetailsByIdAsync(productId, 1, 3);

            var recommendedProducts = await this.productsRepository
                .AllAsNoTracking()
                .Where(x => x.IsMale == product.IsMale && 
                            x.Id != product.Id && 
                            x.Price <= product.Price &&
                            x.Category == product.Category &&
                            x.ProductSizes.Any(x => x.Count != 0))
                .Select(x => new ProductViewModel()
                {
                    Id = x.Id,
                    Category = x.Category,
                    Price = x.Price,
                    Images = x.Images.Select(x => x.Url).Take(2).ToList(),
                    ProductSizes = x.ProductSizes
                    .Where(x => x.Count != 0)
                    .Select(x => x.Size.Name)
                    .ToList()
                })
                .OrderBy(x => Guid.NewGuid())
                .Take(10)
                .ToListAsync();

            return recommendedProducts;
        }
    }
}
