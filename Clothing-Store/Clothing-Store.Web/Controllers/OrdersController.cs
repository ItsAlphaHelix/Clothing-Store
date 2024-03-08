namespace Clothing_Store.Controllers
{
    using Clothing_Store.Core.Contracts;
    using Clothing_Store.Core.ViewModels.Orders;
    using Clothing_Store.Core.ViewModels.Products;
    using Clothing_Store.Core.ViewModels.Shared;
    using Clothing_Store.Data.Data.Models;
    using Clothing_Store.Views.Orders;
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

        [HttpGet]
        public async Task<IActionResult> MineOrders(int page = 1)
        {
            ViewData["IsHomePage"] = false;
            var userId = await GetUserId();
            var model = await this.orderService.GetCustomerWithHisOrdersAsync(userId);

            var paginated = await PaginatedList<OrderViewModel>.CreateAsync(model.OrdersModel, page, 3);

            var viewModel = new PaginatedViewModel<OrderViewModel>()
            {
                Models = paginated,
                CustomerModel = model.CustomerModel
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> OrderDetails(string numberOfOrder, int page = 1)
        {
            ViewData["IsHomePage"] = false;
            ViewData["NumberOfOrder"] = numberOfOrder;
            var products = this.orderService.GetProductsInOrderAsQueryable(numberOfOrder);

            var paginated = await PaginatedList<ProductOrderViewModel>.CreateAsync(products, page, 3);

            var viewModel = new PaginatedViewModel<ProductOrderViewModel>()
            {
                Models = paginated,
            };

            return View(viewModel);
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
