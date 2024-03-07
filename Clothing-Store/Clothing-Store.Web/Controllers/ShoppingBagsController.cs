namespace Clothing_Store.Controllers
{
    using Clothing_Store.Core.Contracts;
    using Clothing_Store.CustomExceptions;
    using Clothing_Store.Data.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    public class ShoppingBagsController : ControllerBase
    {
        private readonly IShoppingBagService shoppingBagService;

        public ShoppingBagsController(
            UserManager<ApplicationUser> usersManager,
            IShoppingBagService shoppingBagService)
            :base(usersManager, shoppingBagService)
        {
            this.shoppingBagService = shoppingBagService;   
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            ViewData["IsHomePage"] = false;
            string userId = await GetUserId();

            var productsInBag = await this.shoppingBagService.GetAllProductsInBagAsync(userId);

            if (!productsInBag.Any())
            {
                return RedirectToAction("Index", "Home");
            }

            return View(productsInBag);
        }


        [HttpPost]
        public async Task<IActionResult> AddProductToBag(int productId, string sizeName, int quantity)
        {

            string userId = await GetUserId();
            try
            {
                await this.shoppingBagService.AddProductToBagAsync(productId, sizeName, quantity, userId);
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
        public async Task<IActionResult> Delete(int bagId)
        {
            await this.shoppingBagService.DeleteProductFromBagAsync(bagId);
            string userId = await GetUserId();
            int countOfProductsInBag = await this.shoppingBagService.CountOfProductsInBagAsync(userId);

            var redirection = countOfProductsInBag == 0 ? RedirectToAction("Index", "Home") : RedirectToAction(nameof(All));

            return redirection;
        }

        [HttpPost]
        public async Task<IActionResult> DecrementQuantityOfProductInBag(string sizeName, int productId)
        {

            string userId = await GetUserId();

            await this.shoppingBagService.DecrementQuantityOfProductAsync(sizeName, productId, userId);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> IncrementQuantityOfProductInBag(string sizeName, int productId, int currentQuantity)
        {

            string userId = await GetUserId();

            try
            {
                await this.shoppingBagService.IncrementQuantityOfProductAsync(sizeName, productId, userId, currentQuantity);
            }
            catch (InvalidOperationException)
            {

                return BadRequest();
            }

            return Ok();
        }
    }
}
