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
        private readonly IOrderService orderService;
        public OrdersController(
            UserManager<ApplicationUser> usersManager,
            IShoppingBagService shoppingBagService,
            IOrderService orderService)
            : base(usersManager, shoppingBagService)
        {
            this.shoppingBagService = shoppingBagService;
            this.orderService = orderService;

        }

        [HttpGet]
        public async Task<IActionResult> Checkout(CustomerViewModel customerViewModel)
        {
            ViewData["IsHomePage"] = false;
            var userId = await GetUserId();
            var productsInBag = await shoppingBagService.GetAllProductsInBagAsync(userId);

            var customer = await this.orderService.SaveInformationAboutCustomerForNextTime(customerViewModel, userId);

            var checkoutModel = new CheckoutViewModel()
            {
                ProductsInBag = productsInBag,
                OrderModel = customer
            };

            return View(checkoutModel);
        }

        [HttpGet]
        public async Task<IActionResult> CompletedOrder()
        {
            ViewData["IsHomePage"] = false;

            var userId = await GetUserId();

            var completedOrder = await this.orderService.CompletedOrderAsync(userId);

            return View(completedOrder);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
        {
            ViewData["IsHomePage"] = false;
            var userId = await GetUserId();

            await this.orderService.CreateOrderAsync(model.OrderModel, userId);

            return RedirectToAction(nameof(CompletedOrder));
        }
        public async Task<IActionResult> Pay()
        {
            ViewData["IsHomePage"] = false;
            return View();
        }

    }
}
