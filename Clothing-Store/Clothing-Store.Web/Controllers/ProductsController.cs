namespace Clothing_Store.Controllers
{
    using Clothing_Store.Core.Contracts;
    using Clothing_Store.Core.ViewModels;
    using Clothing_Store.Core.ViewModels.Products;
    using Clothing_Store.Core.ViewModels.Reviews;
    using Clothing_Store.Data.Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using System.Security.Claims;

    public class ProductsController : Controller
    {
        private readonly IProductService productsService;
        private readonly UserManager<ApplicationUser> usersManager;
        public ProductsController(
            IProductService productsService,
            UserManager<ApplicationUser> usersManager)
        {
            this.productsService = productsService;
            this.usersManager = usersManager;

        }

        [HttpGet]
        public async Task<IActionResult> All([FromQuery] PaginatedViewModel model, int page = 1)
        {
            ViewData["IsHomePage"] = false;
            var products = this.productsService.GetAllProductsAsQueryable(model);

            ViewData["Title"] = "Всички продукти";

            ViewData["CurrentPage"] = page;
            ViewData["CurrentSort"] = model.Sorting;
            ViewData["CurrentSelectedProducts"] = model.SelectedProducts;
            ViewData["CurrentSelectedSizes"] = model.SelectedSizes;
            ViewData["CurrentSelectedPrice"] = model.SelectedPrice;
            var paginated = await PaginatedList<ProductViewModel>.CreateAsync(products, page, 12);


            var viewModel = new PaginatedViewModel()
            {
                Products = paginated
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
        public async Task<IActionResult> AllMenProducts(int page = 1)
        {
            ViewData["IsHomePage"] = false;
            var products = this.productsService.GetAllProductsByGenderAsQueryable(true);

            var paginated = await PaginatedList<ProductViewModel>.CreateAsync(products, page, 12);

            var viewModel = new PaginatedViewModel()
            {
                Products = paginated
            };

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_ProductsPartial", viewModel); // Return only the product list partial view
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> AllWomenProducts(int pageNumber = 1)
        {
            ViewData["IsHomePage"] = false;
            var products = this.productsService.GetAllProductsByGenderAsQueryable(false);

            var paginated = await PaginatedList<ProductViewModel>.CreateAsync(products, pageNumber, 12);

            var viewModel = new PaginatedViewModel()
            {
                Products = paginated
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

                ViewBag.Email = user.Email;
                ViewBag.FullName = user.FullName;
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
        private async Task<ApplicationUser> GetUserAsync()
           => await this.usersManager.FindByIdAsync(this.User.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
    }
}
