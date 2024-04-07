namespace Clothing_Store.Controllers
{
    using Clothing_Store.Core.Contracts;
    using Clothing_Store.Core.ViewModels.Bags;
    using Clothing_Store.Core.ViewModels.Orders;
    using Clothing_Store.Core.ViewModels.Products;
    using Clothing_Store.Core.ViewModels.Shared;
    using Clothing_Store.CustomExceptions;
    using Clothing_Store.Data.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    public class BagsController : ControllerBase
    {
        private readonly IBagsService bagsService;

        public BagsController(
            UserManager<ApplicationUser> usersManager,
            IBagsService bagsService)
            :base(usersManager, bagsService)
        {
            this.bagsService = bagsService;   
        }

        [HttpGet]
        public async Task<IActionResult> All(int page = 1)
        {
            ViewData["IsHomePage"] = false;
            string userId = await GetUserIdAsync();

            var productsInBag = this.bagsService.GetAllProductsInBagAsQueryable(userId);

            if (productsInBag.All(x => x.IsDeleted))
            {
                return RedirectToAction("Index", "Home");
            }

            var paginated = await PaginatedList<ProductBagViewModel>.CreateAsync(productsInBag, page, 3);

            var paginatedView = new PaginatedViewModel<ProductBagViewModel>()
            {
                Models = paginated
            };

            return View(paginatedView);
        }


        [HttpPost]
        public async Task<IActionResult> AddProductToBag(int productId, string sizeName, int quantity)
        {

            string userId = await GetUserIdAsync();
            try
            {
                await this.bagsService.AddProductToBagAsync(productId, sizeName, quantity, userId);
            }
            catch (InvalidSizeException ex)
            {
                ModelState.AddModelError("sizeError", ex.Message);
                return BadRequest(ModelState);
            }
            catch(QuantityException ex)
            {
                ModelState.AddModelError("quantityError", ex.Message);
                return BadRequest(ModelState);
            }

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int productId, string sizeName, int page = 1)
        {
            string userId = await GetUserIdAsync();
            await this.bagsService.DeleteProductFromBagAsync(productId, sizeName, userId);
            var products = this.bagsService.GetAllProductsInBagAsQueryable(userId);
            var paginated = await PaginatedList<ProductBagViewModel>.CreateAsync(products, page, 3);

            if (paginated.Count == 0)
            {
                page--;
            }

            var redirection = page == 0 ? RedirectToAction("Index", "Home") : RedirectToAction(nameof(All), new { page });

            return redirection;
        }

        [HttpPost]
        public async Task<IActionResult> DecrementQuantityOfProductInBag(string sizeName, int productId)
        {

            string userId = await GetUserIdAsync();

            await this.bagsService.DecrementQuantityOfProductAsync(sizeName, productId, userId);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> IncrementQuantityOfProductInBag(string sizeName, int productId, int currentQuantity)
        {

            string userId = await GetUserIdAsync();

            try
            {
                await this.bagsService.IncrementQuantityOfProductAsync(sizeName, productId, userId, currentQuantity);
            }
            catch (InvalidOperationException)
            {

                return BadRequest();
            }

            return Ok();
        }
    }
}
