
namespace Clothing_Store.Controllers
{
    using Clothing_Store.Core.Contracts;
    using Clothing_Store.Core.Services;
    using Clothing_Store.Core.ViewModels.Account;
    using Clothing_Store.Data.Data.Models;
    using Clothing_Store.Data.Repositories;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class AccountsController : Controller
    {
        private readonly IAccountService accountsService;
        private readonly IRepository<ApplicationUser> usersRepository;
        private readonly UserManager<ApplicationUser> usersManager;

        public AccountsController(
            IAccountService accountsService,
            IRepository<ApplicationUser> usersRepository,
            UserManager<ApplicationUser> usersManager)
        {
            this.accountsService = accountsService;
            this.usersRepository = usersRepository;
            this.usersManager = usersManager;
        }

        [HttpGet]
        public IActionResult GetAuthPartialView()
        {
            return PartialView("_AuthPartial");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (this.usersRepository.AllAsNoTracking().Any(x => x.Email.ToLower() == model.Email.ToLower()))
            {
                this.ModelState.AddModelError(string.Empty, "This Email address is already taken.");
            }
            else
            {
                await this.accountsService.RegisterAsync(model);

                //foreach (var error in result.Errors)
                //{
                //    ModelState.AddModelError(string.Empty, error.Description);
                //}
            }
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            //  ModelState.AddModelError(string.Empty, "The username or password you typed are incorrect.");
            if (ModelState.IsValid)
            {
                var result = await this.accountsService.LoginAsync(model);
                if (result.Succeeded)
                {
                    var user = await usersManager.FindByNameAsync(model.Email);

                    return Json(new {user.FullName});
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "The username or password you typed are incorrect.");
                }
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return BadRequest(errors);
        }

        public async Task<IActionResult> Logout()
        {
            await this.accountsService.LogoutAsync();
            return Ok();
        }
    }
}
