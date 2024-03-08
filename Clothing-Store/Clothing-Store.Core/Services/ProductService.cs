namespace Clothing_Store.Core.Services
{
    using Clothing_Store.Core.Contracts;
    using Clothing_Store.Core.ViewModels.Products;
    using Clothing_Store.Core.ViewModels.Reviews;
    using Clothing_Store.Core.ViewModels.Shared;
    using Clothing_Store.Data.Data.Models;
    using Clothing_Store.Data.Repositories;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using System.Web;

    public class ProductService : IProductService
    {
        private readonly IRepository<Product> productsRepository;
        private readonly IRepository<ProductReviews> productReviewsRepository;
        private readonly IRepository<ProductSize> productSizesRepository;
        private readonly IRepository<Size> sizesRepository;
        private readonly UserManager<ApplicationUser> usersManager;
        public ProductService(
            IRepository<Product> productsRepository,
            IRepository<ProductReviews> productReviewsRepository,
            IRepository<ProductSize> productSizes,
            IRepository<Size> sizesRepository,
            UserManager<ApplicationUser> usersManager)
        {
            this.productsRepository = productsRepository;
            this.productReviewsRepository = productReviewsRepository;
            this.productSizesRepository = productSizes;
            this.sizesRepository = sizesRepository;
            this.usersManager = usersManager;
        }

        public IQueryable<ProductViewModel> GetAllProductsByGenderAsQueryable(bool isMen)
        {
            var products = this.productsRepository
                .AllAsNoTracking()
                .Where(x => x.IsMale == isMen)
                .Select(x => new ProductViewModel()
                {
                    Id = x.Id,
                    Category = x.Category,
                    Price = x.Price,
                    Images = x.Images.Select(x => x.Url).Take(2).ToList(),
                })
                .AsQueryable();

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
                    .Select(x => new SizeViewModel() { SizeName = x.Size.Name } )
                    .ToList()
                })
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(model.SelectedProducts))
            {
                products = products.Where(x => model.SelectedProducts.Contains(x.Category));
            }


            if (!string.IsNullOrWhiteSpace(model.SelectedSizes))
            {
               string[] splitSelectedSizes = model.SelectedSizes.Split(",");
               products = products.Where(x => x.ProductSizes.Any(x => splitSelectedSizes.Contains(x.SizeName)));
            }

            if (!string.IsNullOrEmpty(model.SelectedPrice))
            {
                switch (model.SelectedPrice)
                {
                    case "5-15": products = products.Where(x => x.Price >= 5 && x.Price <= 15); break;
                    case "15-30": products = products.Where(x => x.Price >= 15 && x.Price <= 30); break;
                    case "30-50": products = products.Where(x => x.Price >= 30 && x.Price <= 50); break;
                    case "50-100": products = products.Where(x => x.Price >= 50 && x.Price <= 100); break;
                }
            }

            switch (model.Sorting)
            {
                case SortEnum.Default: products = products.AsQueryable(); break;
                case SortEnum.AverageRating: products = products.OrderByDescending(x => x.AverageRating); break;
                case SortEnum.PriceAsc: products = products.OrderBy(x => x.Price); break;
                case SortEnum.PriceDesc: products = products.OrderByDescending(x => x.Price); break;
            }

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
                    .Select(x => new SizeViewModel()
                    {
                        SizeName = x.Size.Name
                    }),
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
                        Date = x.Date
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

            var review = new ProductReviews()
            {
                ProductId = productReview.ProductId,
                UserFullName = user.FullName,
                Rating = productReview.Rating,
                Message = productReview.Message,
                Date = DateTime.Now
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

            double averageRatingOfProduct = productReviews.Any() ? (productReviews.Sum(x => x.Rating) / productReviews.Count()) : 0;

            return averageRatingOfProduct;
        }
        private async Task<int> GetProductReviewsCountAsync(int productId)
        {
            var countOfReviews = await this.productReviewsRepository.AllAsNoTracking()
                .Where(x => x.ProductId == productId)
                .CountAsync();

            return countOfReviews;
        }

    }
}
