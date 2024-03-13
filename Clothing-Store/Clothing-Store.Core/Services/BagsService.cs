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

            int quantityOfCurrentSize = await this.GetTotalQuantityOfSizeOfProduct(sizeName, productId);

            if (quantityOfCurrentSize < quantity)
            {
                throw new QuantityException("Няма достатъчно бройки от този размер.");
            }

            int quantityOfCurrentSizeProductInBag = await this.productsBagRepository
                .AllAsNoTracking()
                .Where(x => x.ProductId == productId && x.SizeName == sizeName && x.Bag.UserId == userId)
                .Select(x => x.Quantity)
                .FirstOrDefaultAsync();

            if (quantityOfCurrentSizeProductInBag >= quantityOfCurrentSize)
            {
                throw new QuantityException("Няма достатъчно бройки от този размер в чантата.");
            }

           
                var currentProduct = await this.productsBagRepository
                    .All()
                    .Where(x => x.Bag.UserId == userId && x.ProductId == productId && x.SizeName == sizeName)
                    .FirstOrDefaultAsync();

                if (currentProduct != null)
                {
                    currentProduct.Quantity += quantity;
                }
                else
                {
                    var bag = new Bag { UserId = userId };

                    var product = await this.productsRepository
                        .All()
                        .Where(x => x.Id == productId)
                        .FirstOrDefaultAsync();

                    var productBag = new ProductBag
                    {
                        Product = product,
                        ProductId = productId,
                        SizeName = sizeName,
                        Bag = bag,
                        BagId = bag.Id,
                        Quantity = quantity,
                    };

                    await bagsRepository.AddAsync(bag);
                    bag.ProductBags.Add(productBag);
                }

                await bagsRepository.SaveChangesAsync();
        }


        public async Task<IEnumerable<BagViewModel>> GetAllProductsInBagAsync(string userId)
        {

            var productsInBag = await this.bagsRepository
                .AllAsNoTracking()
                .Where(x => x.UserId == userId && x.ProductBags.Count != 0)
                .Select(x => new BagViewModel()
                {
                    BagId = x.Id,
                    ProductBags = x.ProductBags.Select(x => new ProductBagViewModel()
                    {
                        Id = x.ProductId,
                        CategoryName = x.Product.Category,
                        Price = x.Product.Price,
                        SizeName = x.SizeName,
                        Quantity = x.Quantity,
                        ImageUrl = x.Product.Images.Select(x => x.Url).FirstOrDefault(),
                        IsProductInStock = x.Product.ProductSizes.Any(x => x.Count != 0)
                    })
                })
                .ToListAsync();

            return productsInBag;
        }
        public async Task<decimal> CalculateTotalPrice(string userId)
        {
            decimal totalPrice = await this.productsBagRepository
                .AllAsNoTracking()
                .Where(x => x.Bag.UserId == userId)
                .SumAsync(x => x.Product.Price * x.Quantity);

            return totalPrice;
        }

        public async Task<int> CountOfProductsInBagAsync(string userId)
        {
            int count = await this.productsBagRepository
                .AllAsNoTracking()
                .Where(x => x.Bag.UserId == userId)
                .SumAsync(x => x.Quantity);

            return count;
        }

        public async Task DeleteProductFromBagAsync(int bagId)
        {
            var bag = await this.bagsRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == bagId);

            this.bagsRepository.Delete(bag);
            await this.bagsRepository.SaveChangesAsync();
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
            var currentProduct = await this.productsBagRepository.All()
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
    }
}
