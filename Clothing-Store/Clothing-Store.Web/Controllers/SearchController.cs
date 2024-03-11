using Clothing_Store.Core.Contracts;
using Clothing_Store.Core.ViewModels.Products;
using Clothing_Store.Core.ViewModels.Shared;
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
        public async Task<IActionResult> ProductsByQuery(
            [FromQuery]PaginatedViewModel<ProductViewModel> model,
            string searchBy,
            int page = 1)
        {
            ViewData["IsHomePage"] = false;
            ViewData["CurrentSearchWord"] = searchBy;
            ViewData["CurrentPage"] = page;
            ViewData["CurrentSort"] = model.Sorting;
            ViewData["CurrentSelectedProducts"] = model.SelectedProducts;
            ViewData["CurrentSelectedSizes"] = model.SelectedSizes;
            ViewData["CurrentSelectedPrice"] = model.SelectedPrice;

            var products = this.searchService.SearchProductsByQueryAsQueryable(model, searchBy);

            if (products == null && !products.Any())
            {
                return NotFound();
            }

            var paginated = await PaginatedList<ProductViewModel>.CreateAsync(products, page, 12);

            var viewModel = new PaginatedViewModel<ProductViewModel>
            {
                Models = paginated
            };

            return View(viewModel);
        }
    }
}
