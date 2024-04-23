using Clothing_Store.Data.Data;

namespace Clothing_Store.Core.Seeding
{
    public interface ISeeder
    {
        Task SeedAsync(ClothingStoreContext dbContext, IServiceProvider serviceProvider);
    }
}
