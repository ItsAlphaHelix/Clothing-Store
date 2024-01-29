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
    }
}
