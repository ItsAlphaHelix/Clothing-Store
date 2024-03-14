namespace Clothing_Store.Controllers
{
    using Clothing_Store.Core.Contracts;
    using Clothing_Store.Core.ViewModels.Orders;
    using Clothing_Store.Core.ViewModels.Shared;
    using Clothing_Store.Data.Data.Models;
    using Clothing_Store.Data.Repositories;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Stripe;

    public class OrdersController : ControllerBase
    {
        private readonly IBagsService shoppingBagService;
        private readonly IOrdersService orderService;
        private readonly IPaymentsService paymentsService;
        private readonly ICustomersService customersService;
        public OrdersController(
            UserManager<ApplicationUser> usersManager,
            IBagsService shoppingBagService,
            IOrdersService orderService,
            IPaymentsService paymentsService,
            ICustomersService customersService)
            : base(usersManager, shoppingBagService)
        {
            this.shoppingBagService = shoppingBagService;
            this.orderService = orderService;
            this.customersService = customersService;
            this.paymentsService = paymentsService;

        }


        [HttpGet]
        public async Task<IActionResult> ChangePaymentMethod(CustomerViewModel customerModel)
        {
            ViewData["IsHomePage"] = false;

            var userId = await GetUserId();

            await this.customersService.ChangeCustomerPaymentMethodAsync(customerModel, userId);

            return RedirectToAction(nameof(Checkout));
        }

        [HttpGet]
        public async Task<IActionResult> Checkout(CustomerViewModel customerModel)
        {
            ViewData["IsHomePage"] = false;

            var userId = await GetUserId();

            var productsInBag = await shoppingBagService.GetAllProductsInBagAsync(userId);

            CheckoutViewModel checkoutModel = new();
            CustomerViewModel customer = null;

            if (this.User?.Identity?.IsAuthenticated ?? false)
            {
                customer = await this.customersService.TakeInformationAboutLoggedInCustomerAsync(userId);
                checkoutModel.CustomerModel = customer;
            }
            else
            {
                customer = await this.customersService.SaveInformationAboutCustomerForNextTimeAsync(customerModel, userId);
                checkoutModel.CustomerModel = customer;
            }

            checkoutModel.ProductsInBag = productsInBag;
            return View(checkoutModel);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
        {
            ViewData["IsHomePage"] = false;
            var userId = await GetUserId();

            await this.orderService.CreateOrderAsync(model.CustomerModel, userId);

            if (model.CustomerModel.IsCustomerWantsToPayOnline == true)
            {
                string sessionUrl = await this.paymentsService.CreateCheckoutSessionAsync(userId);
                return Redirect(sessionUrl);
            }

            return RedirectToAction(nameof(CompletedOrder));
        }

        [HttpGet]
        public async Task<IActionResult> CompletedOrder()
        {
            ViewData["IsHomePage"] = false;

            var userId = await GetUserId();

            var completedOrder = await this.orderService.GetCurrentUserOrderAsync(userId);
            await this.shoppingBagService.DeleteBagsAsync(userId);

            return View(completedOrder);
        }

        [Authorize]
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

        [HttpGet]
        public IActionResult PaymentFailed()
        {
            ViewData["IsHomePage"] = false;
            return View();
        }
    }
}
