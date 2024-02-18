namespace Clothing_Store.Controllers
{
    using Clothing_Store.Core.Contracts;
    using Clothing_Store.Core.ViewModels;
    using Clothing_Store.Core.ViewModels.Products;
    using Clothing_Store.Data.Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewEngines;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using System.Drawing.Printing;
    using System.Linq;
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
        public async Task<IActionResult> All(int page = 1)
        {
            ViewData["IsHomePage"] = false;

            var products = this.productsService.GetAllProductsAsQueryable();

            var paginated = await PaginatedList<ProductViewModel>.CreateAsync(products, page, 12);

            //if (paginated.Count == 0)
            //{
            //    return NotFound();
            //}

            var viewModel = new ProductPaginatedViewModel()
            {
                Products = paginated,
                TotalCount = products.Count()
            };

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_ProductsPartial", viewModel); // Return only the product list partial view
            }

            return View(viewModel);
        }


        [HttpGet]
        public async Task<IActionResult> AllMenProducts(int page = 1)
        {
            ViewData["IsHomePage"] = false;
            var products = this.productsService.GetlAllProductsByGenderAsQueryable(true);

            var paginated = await PaginatedList<ProductViewModel>.CreateAsync(products, page, 12);

            var viewModel = new ProductPaginatedViewModel()
            {
                Products = paginated,
                TotalCount = products.Count()
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
            var products = this.productsService.GetlAllProductsByGenderAsQueryable(false);

            var paginated = await PaginatedList<ProductViewModel>.CreateAsync(products, pageNumber, 12);

            var viewModel = new ProductPaginatedViewModel()
            {
                Products = paginated,
                TotalCount = products.Count()
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

            if (this.signInManager.IsSignedIn(this.User))
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

        public IActionResult Test(int page = 1, int pageSize = 2)
        {
            ViewData["IsHomePage"] = false;

            List<Product> products = new List<Product>()
            { new Product() { Id = 1, Name = "Пуловер", Price = 2.50, CategoryId = 1 },
              new Product() { Id = 2, Name = "Пуловер", Price = 4.00, CategoryId = 1 }, 
              new Product() {Id = 3, Name = "Пуловер", Price = 1.00, CategoryId = 1 },
              new Product() { Id = 4, Name = "Тениска", Price = 15.00, CategoryId = 2 },
              new Product() { Id = 5, Name = "Тениска", Price = 20.00, CategoryId = 2 },
              new Product() {Id = 6, Name = "Тениска", Price = 2.45, CategoryId = 2 },
            };

            int totalProducts = products.Count;
            int totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);
            var paginatedProducts = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var viewModel = new CategoryProductViewModel()
                {
                    Products = paginatedProducts,
                    CurrentPage = page,
                    TotalPages = totalPages
            };

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_TestPartial", viewModel); // Return only the product list partial view
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult FilterTestProducts(string[] queries, int page = 1, int pageSize = 2)
        {

            List<Product> products = new List<Product>()
            { new Product() { Id = 1, Name = "Пуловер", Price = 2.50, CategoryId = 1 },
              new Product() { Id = 2, Name = "Пуловер", Price = 4.00, CategoryId = 1 },
              new Product() {Id = 3, Name = "Пуловер", Price = 1.00, CategoryId = 1 },
              new Product() { Id = 4, Name = "Тениска", Price = 15.00, CategoryId = 2 },
              new Product() { Id = 5, Name = "Тениска", Price = 20.00, CategoryId = 2 },
              new Product() {Id = 6, Name = "Тениска", Price = 2.45, CategoryId = 2 },
            };

            var model = products.Where(x => queries.Contains(x.Name)).ToList();

            int totalProducts = model.Count;
            int totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);
            var paginatedProducts = model.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var viewModel = new CategoryProductViewModel()
            {
                Products = products,
                CurrentPage = page,
                TotalPages = totalPages
            };

           

            // return PartialView("_TestPartial", viewModel);
            return Json(new
            {
                Products = paginatedProducts, // You might need to select into a simpler DTO
                TotalPages = totalPages,
                CurrentPage = page
            });
        }
        private async Task<ApplicationUser> GetUserAsync()
           => await this.usersManager.FindByIdAsync(this.User.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
    }
}
