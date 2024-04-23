using Microsoft.AspNetCore.Mvc;

namespace Clothing_Store.Areas.Administration.Controllers
{
    public class DashboardController : AdministrationController
    {
        [HttpGet("/Administration")]
        public IActionResult Index()
        {
            ViewData["IsHomePage"] = false;
            return View();
        }
    }
}
