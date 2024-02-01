namespace Clothing_Store.Controllers
{
    using Clothing_Store.Core.Contracts;
    using Microsoft.AspNetCore.Mvc;

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
    }
}
