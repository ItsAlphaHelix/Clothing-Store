namespace Clothing_Store.Controllers
{
    using Clothing_Store.Core.Contracts;
    using Clothing_Store.CustomExceptions;
    using Clothing_Store.Data.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;
    using static System.Runtime.InteropServices.JavaScript.JSType;

    public class ShoppingBagsController : Controller
    {
        private readonly UserManager<ApplicationUser> usersManager;
        private readonly IShoppingBagService shoppingBagService;

        public ShoppingBagsController(
            UserManager<ApplicationUser> usersManager,
            IShoppingBagService shoppingBagService)
        {
            this.usersManager = usersManager;
            this.shoppingBagService = shoppingBagService;   
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            ViewData["IsHomePage"] = false;
            string userId = await GetUserId();

            var productsInBag = await this.shoppingBagService.GetAllProductsInBagAsync(userId);

            return View(productsInBag);
        }


        [HttpPost]
        public async Task<IActionResult> AddProductToBag(int productId, string sizeName, int quantity)
        {

            string userId = await GetUserId();
            try
            {
                await this.shoppingBagService.AddProductToBag(productId, sizeName, quantity, userId);
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

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int bagId)
        {
            await this.shoppingBagService.DeleteProductFromBagAsync(bagId);

            return RedirectToAction(nameof(All));
        }
        private async Task<string> GetUserId()
        {
            string temporaryUserId = shoppingBagService.GetOrCreateTemporaryUserId();
            var user = await GetUserAsync();
            string userId = string.Empty;

            if (user == null)
            {
                userId = temporaryUserId;
            }
            else
            {
                userId = user.Id;
            }

            return userId;
        }

        private async Task<ApplicationUser> GetUserAsync()
           => await this.usersManager.FindByIdAsync(this.User.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
    }
}
