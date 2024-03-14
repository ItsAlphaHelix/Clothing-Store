namespace Clothing_Store.Controllers
{
    using Clothing_Store.Core.Contracts;
    using Clothing_Store.Core.ViewModels.Products;
    using Clothing_Store.Core.ViewModels.Reviews;
    using Clothing_Store.Core.ViewModels.Shared;
    using Clothing_Store.Data.Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using System.Security.Claims;

    public class ProductsController : ControllerBase
    {
        private readonly IProductsService productsService;
        public ProductsController(
            IProductsService productsService,
            UserManager<ApplicationUser> usersManager)
            : base(usersManager, null)
        {
            this.productsService = productsService;

        }

        [HttpGet]
        public async Task<IActionResult> All([FromQuery] PaginatedViewModel<ProductViewModel> model, int page = 1)
        {
            ViewData["IsHomePage"] = false;
            ViewData["CurrentPage"] = page;
            ViewData["CurrentSort"] = model.Sorting;
            ViewData["CurrentSelectedProducts"] = model.SelectedProducts;
            ViewData["CurrentSelectedSizes"] = model.SelectedSizes;
            ViewData["CurrentSelectedPrice"] = model.SelectedPrice;

            var products = this.productsService.GetAllProductsAsQueryable(model);

            var paginated = await PaginatedList<ProductViewModel>.CreateAsync(products, page, 12);

            var viewModel = new PaginatedViewModel<ProductViewModel>()
            {
                Models = paginated
            };

            try
            {
                if (!paginated.Any())
                {
                    throw new InvalidOperationException("Няма намерени продукти");
                }
            }
            catch (Exception ex)
            {
                ViewData["Title"] = ex.Message;

                return View(viewModel);
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> AllMenProducts([FromQuery] PaginatedViewModel<ProductViewModel> model, int page = 1)
        {
            ViewData["IsHomePage"] = false;
            ViewData["CurrentPage"] = page;
            ViewData["CurrentSort"] = model.Sorting;
            ViewData["CurrentSelectedProducts"] = model.SelectedProducts;
            ViewData["CurrentSelectedSizes"] = model.SelectedSizes;
            ViewData["CurrentSelectedPrice"] = model.SelectedPrice;

            var products = this.productsService.GetAllProductsByGenderAsQueryable(model, true);

            var paginated = await PaginatedList<ProductViewModel>.CreateAsync(products, page, 12);

            var viewModel = new PaginatedViewModel<ProductViewModel>()
            {
                Models = paginated
            };

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_ProductsPartial", viewModel); // Return only the product list partial view
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> AllWomenProducts([FromQuery] PaginatedViewModel<ProductViewModel> model, int page = 1)
        {
            ViewData["IsHomePage"] = false;
            ViewData["CurrentPage"] = page;
            ViewData["CurrentSort"] = model.Sorting;
            ViewData["CurrentSelectedProducts"] = model.SelectedProducts;
            ViewData["CurrentSelectedSizes"] = model.SelectedSizes;
            ViewData["CurrentSelectedPrice"] = model.SelectedPrice;

            var products = this.productsService.GetAllProductsByGenderAsQueryable(model, false);

            var paginated = await PaginatedList<ProductViewModel>.CreateAsync(products, page, 12);

            var viewModel = new PaginatedViewModel<ProductViewModel>()
            {
                Models = paginated
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetSmallDetails(int productId)
        {
            var product = await this.productsService.GetProductByIdAsync(productId);

            return PartialView("_ProductModalPartial", product);
        }

        [HttpGet]
        public async Task<IActionResult> ProductDetails(int id, int pageNumber, int pageSize = 3)
        {
            ViewData["IsHomePage"] = false;

            if (this.User.Identity.IsAuthenticated)
            {
                var user = await GetUserAsync();

                ViewBag.UserFullName = $"{user.FirstName} {user.LastName}";
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var product = await this.productsService.GetProductDetailsByIdAsync(id, pageNumber, pageSize);
                return PartialView("_ProductReviewsPartial", product);
            }
            else
            {
                var product = await this.productsService.GetProductDetailsByIdAsync(id, 1, pageSize);
                return View(product);
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostProductReview(PostProductReviewViewModel productReview)
        {
            var user = await GetUserAsync();
            
            if (!ModelState.IsValid)
            {

                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }

            await this.productsService.PostProductReviewAsync(productReview, user.Id);
            return Json(productReview);
        }

        public IActionResult IsUserLogin()
        {
            bool isLoggedIn = User.Identity.IsAuthenticated;

            var result = new { IsLoggedIn = isLoggedIn };

            return Ok(result);
        }
    }
}
