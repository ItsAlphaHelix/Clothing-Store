namespace Clothing_Store.Core.Services
{
    using Clothing_Store.Core.Contracts;
    using Clothing_Store.Core.ViewModels.Bags;
    using Clothing_Store.Core.ViewModels.Products;
    using Clothing_Store.Data.Data.Models;
    using Clothing_Store.Data.Repositories;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;

    public class ShoppingBagService : IShoppingBagService
    {
        private readonly IRepository<Bag> bagsRepository;
        private readonly IRepository<ProductBag> productsBagRepository;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ShoppingBagService(
            IRepository<Bag> bagsRepository,
            IRepository<ProductBag> productsBagRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            this.bagsRepository = bagsRepository;
            this.productsBagRepository = productsBagRepository;
            this.httpContextAccessor = httpContextAccessor;

        }
        public async Task AddProductToBag(int productId, string sizeName, int quantity, string userId)
        {
            var userBag = await this.bagsRepository
                .All()
                .Include(b => b.ProductBags)
                .FirstOrDefaultAsync(b => b.UserId == userId);

            if (userBag == null)
            {
                userBag = new Bag()
                {
                    UserId = userId
                };

                await bagsRepository.AddAsync(userBag);
            }

            var productBag = new ProductBag
            {
                ProductId = productId,
                SizeName = sizeName,
                Quantity = quantity,
            };

            userBag.ProductBags.Add(productBag);

            await bagsRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<BagViewModel>> GetAllProductsInBagAsync(string userId)
        {
            var productsInBag = await this.bagsRepository
                .AllAsNoTracking()
                .Where(x => x.UserId == userId)
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
                .SumAsync(x => x.Product.Price);

            return totalPrice;
        }

        public async Task<int> CountOfProductsInBagAsync(string userId)
        {
            int count = await this.productsBagRepository
                .AllAsNoTracking()
                .Where(x => x.Bag.UserId == userId)
                .CountAsync();

            return count;
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

        public async Task DeleteProductFromBagAsync(int bagId)
        {
            var bag = await this.bagsRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == bagId);

            this.bagsRepository.Delete(bag);
            await this.bagsRepository.SaveChangesAsync();
        }
    }
}
