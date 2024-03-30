namespace Clothing_Store.Controllers
{
    using Clothing_Store.Core.Contracts;
    using Clothing_Store.Core.ViewModels.Products;
    using Clothing_Store.Core.ViewModels.Shared;
    using Clothing_Store.Data.Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class FavoritesController : ControllerBase
    {
        private readonly IFavoritesService favoritesService;
        public FavoritesController(
            UserManager<ApplicationUser> usersManager,
            IFavoritesService favoritesService)
            : base(usersManager, null)
        {
            this.favoritesService = favoritesService;
        }
       
        public async Task<IActionResult> All(int page = 1)
        {
            ViewData["IsHomePage"] = false;

            var user = await GetUserAsync();

            var products = this.favoritesService.AllFavoritesProductsAsQueryable(user.Id);

            if (products.All(x => x.IsDeleted))
            {
                return RedirectToAction("Index", "Home");
            }

            var paginated = await PaginatedList<ProductViewModel>.CreateAsync(products, page, 3);


            var paginatedView = new PaginatedViewModel<ProductViewModel>()
            {
                Models = paginated
            };

            return View(paginatedView);
        }

        public async Task<IActionResult> Add(
            int id, string sorting, string selectedProducts, string selectedPrice, string selectedSizes, int page = 1)
        {
            ViewData["IsHomePage"] = false;

            var user = await GetUserAsync();

            await this.favoritesService.AddFavoriteProduct(user.Id, id);

            var currentContextUrl = Request.Headers["Referer"].ToString();

            if (currentContextUrl.Contains("ProductDetails"))
            {
                return Redirect(currentContextUrl);
            }
            else if (currentContextUrl.Contains("Bags"))
            {
                return Redirect(currentContextUrl);
            }

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

        public async Task<IActionResult> Delete(int productId, int page)
        {
            ViewData["IsHomePage"] = false;

            await this.favoritesService.DeleteFavoriteProduct(productId);

            string userId = GetUserAsync().Result.Id;
            var products =  this.favoritesService.AllFavoritesProductsAsQueryable(userId);
            var paginated = await PaginatedList<ProductViewModel>.CreateAsync(products, page, 3);

            if (paginated.Count == 0)
            {
                page--;
            }

            var redirection = page == 0 ? RedirectToAction("Index", "Home") : RedirectToAction(nameof(All), new { page });

            return redirection;
        }
    }
}
