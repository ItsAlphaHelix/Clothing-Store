namespace Clothing_Store.Core.Services
{
    using Clothing_Store.Core.Contracts;
    using Clothing_Store.Core.ViewModels.Bags;
    using Clothing_Store.Core.ViewModels.Products;
    using Clothing_Store.CustomExceptions;
    using Clothing_Store.Data.Data.Models;
    using Clothing_Store.Data.Repositories;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class BagsService : IBagsService
    {
        private readonly IRepository<Bag> bagsRepository;
        private readonly IRepository<ProductBag> productsBagRepository;
        private readonly IRepository<ProductSize> productsSizeRepository;
        private readonly IRepository<Product> productsRepository;
        private readonly IHttpContextAccessor httpContextAccessor;

        public BagsService(
            IRepository<Bag> bagsRepository,
            IRepository<ProductBag> productsBagRepository,
            IRepository<ProductSize> productsSizeRepository,
            IRepository<Product> productsRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            this.bagsRepository = bagsRepository;
            this.productsBagRepository = productsBagRepository;
            this.productsSizeRepository = productsSizeRepository;
            this.productsRepository = productsRepository;
            this.httpContextAccessor = httpContextAccessor;

        }
        public async Task AddProductToBagAsync(int productId, string sizeName, int quantity, string userId)
        {

            if (string.IsNullOrWhiteSpace(sizeName))
            {
                throw new InvalidSizeException("Моля изберете размер.");
            }

            var bag = await bagsRepository
                .All()
                .Include(x => x.ProductBags)
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync();

            if (bag == null)
            {
                bag = new Bag
                {
                    UserId = userId,
                    CreatedOn = DateTime.UtcNow
                };

                await bagsRepository.AddAsync(bag);
            }

            var productBag = await this.productsBagRepository
                    .All()
                    .Where(x => x.Bag.UserId == userId && x.ProductId == productId)
                    .FirstOrDefaultAsync();

            if (productBag == null)
            {
                var product = await this.productsRepository
                    .All()
                    .Where(x => x.Id == productId)
                    .FirstOrDefaultAsync();

                productBag = new ProductBag
                {
                    Product = product,
                    ProductId = productId,
                    SizeName = sizeName,
                    Bag = bag,
                    BagId = bag.Id,
                    Quantity = quantity,
                    CreatedOn = DateTime.UtcNow
                };

                bag.ProductBags.Add(productBag);
            }

            int quantityOfCurrentSize = await this.GetTotalQuantityOfSizeOfProduct(sizeName, productId);

            int quantityOfCurrentSizeProductInBag = await this.productsBagRepository
                .AllAsNoTracking()
                .Where(x => x.ProductId == productId && x.SizeName == sizeName && x.Bag.UserId == userId)
                .Select(x => x.Quantity)
                .FirstOrDefaultAsync();

            if (quantityOfCurrentSizeProductInBag + quantity > quantityOfCurrentSize)
            {
                throw new QuantityException("Няма достатъчно бройки от този размер.");
            }


            var currentProduct = await this.productsBagRepository
                .All()
                .Where(x => x.Bag.UserId == userId && x.ProductId == productId && x.SizeName == sizeName)
                .FirstOrDefaultAsync();

            if (currentProduct != null)
            {
                currentProduct.Quantity += quantity;
            }

            productBag.IsDeleted = false;
            productBag.ModifiedOn = DateTime.UtcNow;

            await bagsRepository.SaveChangesAsync();
        }


        public IQueryable<ProductBagViewModel> GetAllProductsInBagAsQueryable(string userId)
        {

            var products = this.productsBagRepository
                .AllAsNoTracking()
                .Where(x => x.Bag.UserId == userId && !x.IsDeleted)
                .OrderByDescending(x => x.ProductId)
                .Select(x => new ProductBagViewModel()
                {
                    Id = x.ProductId,
                    CategoryName = x.Product.Category,
                    Price = x.Product.Price,
                    SizeName = x.SizeName,
                    Quantity = x.Quantity,
                    ImageUrl = x.Product.Images.Select(x => x.Url).FirstOrDefault(),
                    IsProductInStock = x.Product.ProductSizes.Any(x => x.Count != 0),
                    IsDeleted = x.IsDeleted
                })
                .AsQueryable();

            return products;
        }
        public async Task<decimal> CalculateTotalPrice(string userId)
        {
            decimal totalPrice = await this.productsBagRepository
                .AllAsNoTracking()
                .Where(x => x.Bag.UserId == userId && !x.IsDeleted)
                .SumAsync(x => x.Product.Price * x.Quantity);

            return totalPrice;
        }

        public async Task<int> CountOfProductsInBagAsync(string userId)
        {
            int count = await this.productsBagRepository
                .AllAsNoTracking()
                .Where(x => x.Bag.UserId == userId && !x.IsDeleted)
                .SumAsync(x => x.Quantity);

            return count;
        }

        public async Task DeleteProductFromBagAsync(int? productId, string userId)
        {
            var productsInBag = await this.productsBagRepository
                .All()
                .Include(x => x.Bag)
                .Where(x => x.ProductId == productId || x.Bag.UserId == userId)
                .ToListAsync();

            foreach (var productInBag in productsInBag)
            {
                productInBag.IsDeleted = true;
                productInBag.Quantity = 0;
                productInBag.DeletedOn = DateTime.UtcNow;
            }

            await this.productsBagRepository.SaveChangesAsync();
        }

        public async Task<int> GetTotalQuantityOfSizeOfProduct(string sizeName, int productId)
        {
            int quantityOfCurrentSize = await this.productsSizeRepository
                .AllAsNoTracking()
                .Where(x => x.ProductId == productId && x.Size.Name == sizeName)
                .Select(x => x.Count)
                .FirstOrDefaultAsync();

            return quantityOfCurrentSize;
        }

        public async Task DecrementQuantityOfProductAsync(string sizeName, int productId, string userId)
        {
            var currentProduct = await this.productsBagRepository
                .All()
                .Where(x => x.Bag.UserId == userId && x.ProductId == productId && x.SizeName == sizeName)
                .FirstOrDefaultAsync();

            currentProduct.Quantity--;

            await this.productsBagRepository.SaveChangesAsync();
        }

        public async Task IncrementQuantityOfProductAsync(string sizeName, int productId, string userId, int currentQuantity)
        {
            var currentProduct = await this.productsBagRepository.All()
                .Where(x => x.Bag.UserId == userId && x.ProductId == productId && x.SizeName == sizeName)
                .FirstOrDefaultAsync();

            int quantityOfCurrentSize = await this.GetTotalQuantityOfSizeOfProduct(sizeName, productId);
            currentProduct.Quantity++;

            if (quantityOfCurrentSize <= currentQuantity)
            {
                throw new InvalidOperationException("Няма достатъчно бройки от този размер.");
            }


            await this.productsBagRepository.SaveChangesAsync();
        }
        public string GetOrCreateTemporaryUserId()
        {
            var httpContext = this.httpContextAccessor.HttpContext;

            if (httpContext.Request.Cookies.ContainsKey("TemporaryUserId"))
            {
                return httpContext.Request.Cookies["TemporaryUserId"];
            }
            else
            {
                var temporaryUserId = Guid.NewGuid().ToString();

                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddMonths(1),
                    HttpOnly = true,
                };

                httpContext.Response.Cookies.Append("TemporaryUserId", temporaryUserId, cookieOptions);

                return temporaryUserId;
            }
        }
        public async Task<IEnumerable<ProductViewModel>> GetRecommendedProductsInBag(string userId)
        {
            var productsInBag = this.GetAllProductsInBagAsQueryable(userId);

            var maxPrice = await productsInBag.MaxAsync(x => (x.Price * x.Quantity));


            var recommendedProducts = await this.productsRepository
               .AllAsNoTracking()
               .Where(x => x.Price <= maxPrice &&
                           x.ProductSizes.Any(x => x.Count != 0) &&
                           productsInBag.All(pb => pb.Id != x.Id))
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
