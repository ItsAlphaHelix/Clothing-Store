namespace Clothing_Store.Extensions
{
    using AspNetCoreHero.ToastNotification;
    using Clothing_Store.Core.Contracts;
    using Clothing_Store.Core.Services;
    using Clothing_Store.Core.WebScrapper;
    using Clothing_Store.Data.Repositories;

    public static class ClothingStoreServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IProductsService, ProductsService>();
            services.AddScoped<ISearchService, SearchService>();
            services.AddScoped<IFavoritesService, FavoritesService>();
            services.AddScoped<IBagsService, BagsService>();
            services.AddScoped<IOrdersService, OrdersService>();
            services.AddScoped<IPaymentsService, PaymentsService>();
            services.AddScoped<ICustomersService, CustomersService>();
            services.AddScoped<IScrape, Scrape>();
            services.AddNotyf(configuration =>
            {
                configuration.DurationInSeconds = 5;
                configuration.IsDismissable = true;
                configuration.Position = NotyfPosition.TopRight;
            });

            return services;
        }
    }
}
