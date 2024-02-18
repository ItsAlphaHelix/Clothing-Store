using Clothing_Store.Core.Contracts;
using Clothing_Store.Core.ViewModels;
using Clothing_Store.Core.ViewModels.Products;
using Microsoft.AspNetCore.Mvc;

namespace Clothing_Store.Controllers
{
    public class SearchController : Controller
    {
        private readonly ISearchService searchService;
        public SearchController(ISearchService searchService)
        {
            this.searchService = searchService;
        }

        [HttpGet]
        public async Task<IActionResult> ProductsByQuery(string query, int pageNumber = 1)
        {
            ViewData["IsHomePage"] = false;
            this.ViewData["CurrentSearchWord"] = query;

            var products = this.searchService.SearchProductsByQueryAsQueryable(query);

            if (products == null && !products.Any())
            {
                return NotFound();
            }

            var paginated = await PaginatedList<ProductViewModel>.CreateAsync(products, pageNumber, 12);

            var viewModel = new ProductPaginatedViewModel
            {
                Products = paginated,
                TotalCount = products.Count(),
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> FilterProducts(string[] queries, int page = 1)
        {
            ViewData["IsHomePage"] = false;
            //this.ViewData["CurrentSearchWord"] = query;

            var products = this.searchService.FilterProductsAsQueryable(queries);

            if (products == null && !products.Any())
            {
                return NotFound();
            }

            var paginatedProducts = await PaginatedList<ProductViewModel>.CreateAsync(products, page, 12);

            return Json(new
            {
                Products = paginatedProducts,
                paginatedProducts.TotalPages,
                Page = paginatedProducts.PageNumber
            });
        }
    }
}
