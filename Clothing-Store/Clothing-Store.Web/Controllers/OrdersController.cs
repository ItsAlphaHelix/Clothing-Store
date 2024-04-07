namespace Clothing_Store.Controllers
{
    using Clothing_Store.Core.Contracts;
    using Clothing_Store.Core.ViewModels.Customers;
    using Clothing_Store.Core.ViewModels.Orders;
    using Clothing_Store.Core.ViewModels.Shared;
    using Clothing_Store.Data.Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using Stripe.Checkout;

    public class OrdersController : ControllerBase
    {
        private readonly IBagsService bagsService;
        private readonly IOrdersService ordersService;
        private readonly IPaymentsService paymentsService;
        private readonly ICustomersService customersService;
        public OrdersController(
            UserManager<ApplicationUser> usersManager,
            IBagsService bagsService,
            IOrdersService ordersService,
            IPaymentsService paymentsService,
            ICustomersService customersService)
            : base(usersManager, bagsService)
        {
            this.bagsService = bagsService;
            this.ordersService = ordersService;
            this.customersService = customersService;
            this.paymentsService = paymentsService;

        }

        [HttpGet]
        public async Task<IActionResult> RefundOrder(string numberOfOrder)
        {
            ViewData["IsHomePage"] = false;

            await this.paymentsService.RefundAsync(numberOfOrder);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Checkout(CustomerViewModel customerModel)
        {
            ViewData["IsHomePage"] = false;

            var userId = await GetUserIdAsync();

            var productsInBag = bagsService.GetAllProductsInBagAsQueryable(userId);

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
            var userId = await GetUserIdAsync();

            if (model.CustomerModel.IsCustomerWantsToPayOnline == true)
            {
                var session = await this.paymentsService.CreateCheckoutSessionAsync(userId);

                TempData["Model"] = JsonConvert.SerializeObject(model);
                TempData["Session"] = session.Id;

                return Redirect(session.Url);
            }

            await this.ordersService.CreateOrderAsync(model.CustomerModel, userId);
            await this.bagsService.DeleteAllProductsFromBagAsync(userId);

            return RedirectToAction(nameof(OrderConfirmation));
        }

        [HttpGet]
        public async Task<IActionResult> OrderConfirmation()
        {
            ViewData["IsHomePage"] = false;

            var userId = await GetUserIdAsync();

            var service = new SessionService();

            if (TempData["Session"] != null)
            {
                var session = await service.GetAsync(TempData["Session"].ToString());

                if (session.PaymentStatus == "paid")
                {
                    var checkoutModel = JsonConvert.DeserializeObject<CheckoutViewModel>(TempData["Model"].ToString());
                    
                    await this.ordersService.CreateOrderAsync(
                        checkoutModel.CustomerModel,
                        userId,
                        session.PaymentStatus,
                        session.Id, 
                        session.PaymentIntentId);

                    await this.bagsService.DeleteAllProductsFromBagAsync(userId);
                }
            }

            var completedOrder = await this.ordersService.GetCurrentUserOrderAsync(userId);

            return View(completedOrder);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> MineOrders(int page = 1)
        {
            ViewData["IsHomePage"] = false;
            var userId = await GetUserIdAsync();
            IQueryable<OrderViewModel> mineOrders;

            try
            {
                mineOrders = this.ordersService.GetCustomerOrdersAsQueryable(userId);
            }
            catch (NullReferenceException)
            {
                return RedirectToAction("Index", "Home");
            }

            var paginated = await PaginatedList<OrderViewModel>.CreateAsync(mineOrders, page, 3);

            var viewModel = new PaginatedViewModel<OrderViewModel>()
            {
                Models = paginated,
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> OrderDetails(string numberOfOrder, int page = 1)
        {
            ViewData["IsHomePage"] = false;
            ViewData["NumberOfOrder"] = numberOfOrder;
            var products = this.ordersService.GetProductsInOrderAsQueryable(numberOfOrder);

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
