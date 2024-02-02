namespace Clothing_Store.Controllers
{
    using Clothing_Store.Core.Contracts;
    using Clothing_Store.Core.ViewModels.Products;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Infrastructure;

    public class ProductsController : Controller
    {
        private readonly IProductService productsService;
        public ProductsController(IProductService productsService)
        {
            this.productsService = productsService;
        }
        public async Task<IActionResult> All()
        {
            ViewBag.IsHomePage = false;

            var products = await this.productsService.GetAllProductsAsync();
            return View(products);
        }

        public async Task<IActionResult> AllMenProducts()
        {
            ViewBag.IsHomePage = false;
            var products = await this.productsService.GetlAllProductsByGenderAsync(true);

            return View(products);
        }

        public async Task<IActionResult> AllWomenProducts()
        {
            ViewBag.IsHomePage = false;
            var products = await this.productsService.GetlAllProductsByGenderAsync(false);

            return View(products);
        }
        public async Task<IActionResult> GetSmallDetails(int productId)
        {
            var product = await this.productsService.GetProductByIdAsync(productId);

            return PartialView("_ProductModalPartial", product);
        }

        public async Task<IActionResult> ProductDetails(int id)
        {
            ViewBag.IsHomePage = false;

            var product = await this.productsService.GetProductDetailsByIdAsync(id);

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> PostProductReview(PostProductReviewViewModel productReview)
        {

            if (ModelState.IsValid)
            {
                await this.productsService.PostProductReviewAsync(productReview);
                return Json(productReview); ;
            }

            return RedirectToAction(nameof(ProductDetails));
        }

        //public async Task<IActionResult> GetProductReviews(int productId)
        //{
        //    var reviews = await this.productsService.GetReviewsForProductAsync(productId);

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest();
        //    }

        //    return PartialView("_GetReviewsPartial", reviews);
        //}
    }
}
