namespace Clothing_Store.Controllers
{
    using Clothing_Store.Core.Contracts;
    using Clothing_Store.Core.ViewModels;
    using Clothing_Store.Core.ViewModels.Favorites;
    using Clothing_Store.Data.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Drawing.Printing;
    using System.Security.Claims;

    public class FavoritesController : Controller
    {
        private readonly UserManager<ApplicationUser> usersManager;
        private readonly IFavoriteService favoriteService;
        public FavoritesController(
            UserManager<ApplicationUser> usersManager,
            IFavoriteService favoriteService)
        {
            this.usersManager = usersManager;
            this.favoriteService = favoriteService;
        }
        public async Task<IActionResult> All(int page = 1)
        {
            ViewData["IsHomePage"] = false;

            string userId = GetUserAsync().Result.Id;

            var favoritesProducts = this.favoriteService.AllFavoritesProductsAsync(userId);

            var paginated = await PaginatedList<FavoriteViewModel>.CreateAsync(favoritesProducts, page, 3);

            var paginatedView = new FavoritePaginatedViewModel()
            {
                Favorites = paginated
            };

            return View(paginatedView);
        }

        public async Task<IActionResult> Add(int id)
        {
            ViewData["IsHomePage"] = false;

            string userId = GetUserAsync().Result.Id;

            await this.favoriteService.AddFavoriteProduct(userId, id);
            var favoritesProducts = this.favoriteService.AllFavoritesProductsAsync(userId);
            int pageSize = 3;
            var page = (int)Math.Ceiling(favoritesProducts.Count() / (double)pageSize);

            return RedirectToAction(nameof(All), new { page });
        }

        public async Task<IActionResult> Delete(int favoriteId, int page)
        {
            ViewData["IsHomePage"] = false;

            await this.favoriteService.DeleteFavoriteProduct(favoriteId);

            string userId = GetUserAsync().Result.Id;
            var favoritesProducts = this.favoriteService.AllFavoritesProductsAsync(userId);
            var paginated = await PaginatedList<FavoriteViewModel>.CreateAsync(favoritesProducts, page, 3);

            if (paginated.Count == 0)
            {
                page--;
            }

            return RedirectToAction(nameof(All), new { page });
        }
        private async Task<ApplicationUser> GetUserAsync()
           => await this.usersManager.FindByIdAsync(this.User.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
    }
}
