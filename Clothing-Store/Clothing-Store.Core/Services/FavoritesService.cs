namespace Clothing_Store.Core.Services
{
    using Clothing_Store.Core.Contracts;
    using Clothing_Store.Core.ViewModels.Products;
    using Clothing_Store.Data.Data.Models;
    using Clothing_Store.Data.Repositories;
    using Microsoft.EntityFrameworkCore;

    public class FavoritesService : IFavoritesService
    {
        private readonly IRepository<Product> productsRepository;
        private readonly IRepository<Favorite> favoritesRepository;
        private readonly IRepository<ProductFavorites> productFavoritesRepository;
        public FavoritesService(
            IRepository<Product> productsRepository,
            IRepository<Favorite> favoritesRepository,
            IRepository<ProductFavorites> productFavoritesRepository)
        {
            this.productsRepository = productsRepository;
            this.favoritesRepository = favoritesRepository;
            this.productFavoritesRepository = productFavoritesRepository;

        }
        public async Task AddFavoriteProduct(string userId, int productId)
        {
            var favorite = await this.favoritesRepository
                .All()
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync();

            if (favorite == null)
            {
                favorite = new Favorite()
                {
                    UserId = userId,
                    CreatedOn = DateTime.UtcNow,
                };

                await this.favoritesRepository.AddAsync(favorite);
            }

            var productFavorite = await productFavoritesRepository
                .All()
                .Where(x => x.ProductId == productId && x.Favorite.UserId == userId)
                .FirstOrDefaultAsync();

            if (productFavorite == null)
            {
                var product = await productsRepository
                    .AllAsNoTracking()
                    .Where(x => x.Id == productId)
                    .FirstOrDefaultAsync();

                productFavorite = new ProductFavorites()
                {
                    Favorite = favorite,
                    FavoriteId = favorite.Id,
                    Product = product,
                    ProductId = productId,
                    CreatedOn = DateTime.UtcNow
                };

                favorite.ProductFavorites.Add(productFavorite);
            }

            productFavorite.IsDeleted = false;
            productFavorite.ModifiedOn = DateTime.UtcNow;

            await favoritesRepository.SaveChangesAsync();
        }
        public IQueryable<ProductViewModel> AllFavoritesProductsAsQueryable(string userId)
        {
            var products = productFavoritesRepository
                .AllAsNoTracking()
                .Where(x => x.Favorite.UserId == userId && !x.IsDeleted)
                .OrderByDescending(x => x.ProductId)
                .Select(x => new ProductViewModel()
                {
                    Id = x.ProductId,
                    Images = x.Product.Images.Select(x => x.Url).ToList(),
                    Category = x.Product.Category,
                    Price = x.Product.Price,
                    IsProductInStock = x.Product.ProductSizes.Any(x => x.Count != 0),
                    IsDeleted = x.IsDeleted
                })
                .AsQueryable();


            return products;
        }

        public async Task<int> CountOfFavoriteProductsAsync(string userId)
        {
            var countOfFavoriteProducts = await this.productFavoritesRepository
                .AllAsNoTracking()
                .Where(x => x.Favorite.UserId == userId && !x.IsDeleted)
                .CountAsync();

            return countOfFavoriteProducts;
        }

        public async Task<IEnumerable<ProductViewModel>> AllUserFavoriteProductsAsync(string userId)
        {
            var products = await this.productsRepository.AllAsNoTracking()
                .Select(x => x.Id)
                .ToListAsync();

            var favoriteProducts = await this.productFavoritesRepository
                .AllAsNoTracking()
                .Where(x => x.Favorite.UserId == userId && products.Contains(x.ProductId) && !x.IsDeleted)
                .Select(x => new ProductViewModel() { Id = x.ProductId })
                .ToListAsync();

            if (favoriteProducts != null && favoriteProducts.Any())
            {
                return favoriteProducts;
            }

            return favoriteProducts;
        }

        public async Task DeleteFavoriteProduct(int productId)
        {
            var favoriteProduct = await this.productFavoritesRepository
                .All()
                .FirstOrDefaultAsync(x => x.ProductId == productId);

            favoriteProduct.IsDeleted = true;
            favoriteProduct.DeletedOn = DateTime.UtcNow;

            await favoritesRepository.SaveChangesAsync();
        }
    }
}
