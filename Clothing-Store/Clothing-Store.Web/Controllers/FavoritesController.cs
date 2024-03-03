namespace Clothing_Store.Controllers
{
    using Clothing_Store.Core.Contracts;
    using Clothing_Store.Core.ViewModels;
    using Clothing_Store.Core.ViewModels.Favorites;
    using Clothing_Store.Data.Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    [Authorize]
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

            if (!favoritesProducts.Any())
            {
                return RedirectToAction("Index", "Home");
            }

            var paginated = await PaginatedList<FavoriteViewModel>.CreateAsync(favoritesProducts, page, 3);

            var paginatedView = new FavoritePaginatedViewModel()
            {
                Favorites = paginated
            };

            return View(paginatedView);
        }

        public async Task<IActionResult> Add(
            int id, int page, string sorting, string selectedProducts, string selectedPrice, string selectedSizes)
        {
            ViewData["IsHomePage"] = false;

            string userId = GetUserAsync().Result.Id;

            await this.favoriteService.AddFavoriteProduct(userId, id);

            var redirection = new
            {
                page,
                sorting,
                selectedProducts,
                selectedPrice,
                selectedSizes
            };
            return RedirectToAction("All", "Products", redirection);
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

            var redirection = page == 0 ? RedirectToAction("Index", "Home") : RedirectToAction(nameof(All), new { page });

            return redirection;
        }
        private async Task<ApplicationUser> GetUserAsync()
           => await this.usersManager.FindByIdAsync(this.User.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
    }
}
