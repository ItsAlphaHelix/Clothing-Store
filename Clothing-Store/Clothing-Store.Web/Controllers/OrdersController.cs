namespace Clothing_Store.Controllers
{
    using Clothing_Store.Core.Contracts;
    using Clothing_Store.Core.ViewModels.Orders;
    using Clothing_Store.Data.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    public class OrdersController : ControllerBase
    {
        private readonly IShoppingBagService shoppingBagService;
        public OrdersController(
            UserManager<ApplicationUser> usersManager,
            IShoppingBagService shoppingBagService)
            :base(usersManager, shoppingBagService)
        {
            this.shoppingBagService = shoppingBagService;
        }
        public async Task<IActionResult> Checkout()
        {
            ViewData["IsHomePage"] = false;
            var userId = await GetUserId();
            var productsInBag = await shoppingBagService.GetAllProductsInBagAsync(userId);

            var checkoutModel = new CheckoutViewModel()
            {
                ProductsInBag = productsInBag
            };

            return View(checkoutModel);
        }

        public async Task<IActionResult> Pay()
        {
            ViewData["IsHomePage"] = false;
            return View();
        }

    }
}
