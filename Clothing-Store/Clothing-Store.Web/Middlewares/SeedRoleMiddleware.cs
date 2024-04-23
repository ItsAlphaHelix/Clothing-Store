using Clothing_Store.Core.Seeding;
using Clothing_Store.Data.Data;

namespace Clothing_Store.Middlewares
{
    public static class SeedRoleMiddlewareExtension
    {
        public static WebApplication Seed(
        this WebApplication app)
        {
            var serviceScope = app.Services.CreateScope();
            var dbContext = serviceScope.ServiceProvider.GetRequiredService<ClothingStoreContext>();
            new ClothingStoreContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();

            return app;
        }
    }
}
