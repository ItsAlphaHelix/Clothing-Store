namespace Clothing_Store.Controllers
{
    using Clothing_Store.Core.Contracts;
    using Clothing_Store.Core.ViewModels.Products;
    using Clothing_Store.Data.Data.Models;
    using HtmlAgilityPack;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    public class ProductsController : Controller
    {
        private readonly IProductService productsService;
        private readonly UserManager<ApplicationUser> usersManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        public ProductsController(
            IProductService productsService,
            UserManager<ApplicationUser> usersManager,
            SignInManager<ApplicationUser> signInManager)
        {
            this.productsService = productsService;
            this.usersManager = usersManager;
            this.signInManager = signInManager;

        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            ViewBag.IsHomePage = false;

            var products = await this.productsService.GetAllProductsAsync();
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> AllMenProducts()
        {
            ViewBag.IsHomePage = false;
            var products = await this.productsService.GetlAllProductsByGenderAsync(true);

            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> AllWomenProducts()
        {
            ViewBag.IsHomePage = false;
            var products = await this.productsService.GetlAllProductsByGenderAsync(false);

            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> GetSmallDetails(int productId)
        {
            var product = await this.productsService.GetProductByIdAsync(productId);

            return PartialView("_ProductModalPartial", product);
        }

        [HttpGet]
        public async Task<IActionResult> ProductDetails(int id)
        {
            ViewBag.IsHomePage = false;

            if (this.signInManager.IsSignedIn(this.User))
            {
                var user = await GetUserAsync();

                ViewBag.Email = user.Email;
                ViewBag.FullName = user.FullName;
            }

            var product = await this.productsService.GetProductDetailsByIdAsync(id);

            return View(product);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostProductReview(PostProductReviewViewModel productReview)
        {
            var user = await GetUserAsync();

            if (ModelState.IsValid)
            {
                await this.productsService.PostProductReviewAsync(productReview, user.Id);
                return Json(productReview);
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return BadRequest(errors);
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
