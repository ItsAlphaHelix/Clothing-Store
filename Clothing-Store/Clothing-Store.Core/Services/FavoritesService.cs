namespace Clothing_Store.Core.Services
{
    using Clothing_Store.Core.Contracts;
    using Clothing_Store.Core.ViewModels.Favorites;
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
                .Where(x => x.UserId == userId && x.ProductFavorites.Any(x => x.ProductId == productId))
                .FirstOrDefaultAsync();

            if (favorite == null)
            {
                favorite = new Favorite()
                {
                    UserId = userId,
                    CreatedOn = DateTime.UtcNow,
                };

                var product = await productsRepository
                    .All()
                    .Where(x => x.Id == productId)
                    .FirstOrDefaultAsync();

                var productFavorite = new ProductFavorites()
                {
                    Favorite = favorite,
                    FavoriteId = favorite.Id,
                    Product = product,
                    ProductId = productId
                };


                favorite.ProductFavorites.Add(productFavorite);
                await this.favoritesRepository.AddAsync(favorite);
            }
            else
            {
                favorite.IsDeleted = false;
                favorite.ModifiedOn = DateTime.UtcNow;
            }

            await favoritesRepository.SaveChangesAsync();
        }

        public IQueryable<FavoriteViewModel> AllFavoritesProductsAsync(string userId)
        {
            var favorites = this.favoritesRepository
                .AllAsNoTracking()
                .Where(x => x.UserId == userId && !x.IsDeleted)
                .Select(x => new FavoriteViewModel()
                {
                    Id = x.Id,
                    FavoriteProducts = x.ProductFavorites
                    .Select(x => new ProductViewModel()
                    {
                        Id = x.ProductId,
                        Images = x.Product.Images.Select(x => x.Url).ToList(),
                        Category = x.Product.Category,
                        Price = x.Product.Price,
                        IsProductInStock = x.Product.ProductSizes.Any(x => x.Count != 0)
                    }).ToList()
                }).AsQueryable();

            return favorites;
        }

        public async Task<int> CountOfFavoriteProductsAsync(string userId)
        {
            var countOfFavoriteProducts = await this.favoritesRepository
                .AllAsNoTracking()
                .Where(x => x.UserId == userId && !x.IsDeleted)
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
                .Where(x => x.Favorite.UserId == userId && products.Contains(x.ProductId) && !x.Favorite.IsDeleted)
                .Select(x => new ProductViewModel() { Id = x.ProductId })
                .ToListAsync();

            if (favoriteProducts != null && favoriteProducts.Any())
            {
                return favoriteProducts;
            }

            return favoriteProducts;
        }

        public async Task DeleteFavoriteProduct(int favoriteId)
        {
            var favoriteProduct = await this.favoritesRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == favoriteId);

            favoriteProduct.IsDeleted = true;
            favoriteProduct.DeletedOn = DateTime.UtcNow;

            await favoritesRepository.SaveChangesAsync();
        }
    }
}
