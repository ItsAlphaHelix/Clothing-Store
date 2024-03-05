namespace Clothing_Store.Extensions
{
    using Clothing_Store.Core.Contracts;
    using Clothing_Store.Core.Services;
    using Clothing_Store.Data.Repositories;
    public static class ClothingStoreServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ISearchService, SearchService>();
            services.AddScoped<IFavoriteService, FavoriteService>();
            services.AddScoped<IShoppingBagService, ShoppingBagService>();
            services.AddScoped<IOrderService, OrderService>();

            return services;
        }
    }
}
