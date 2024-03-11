namespace Clothing_Store.Areas.Identity.Pages.Account
{
    #nullable disable

    using Clothing_Store.Data.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<LogoutModel> logger;

        public LogoutModel(
            SignInManager<ApplicationUser> signInManager,
            ILogger<LogoutModel> logger)
        {
            this.signInManager = signInManager;
            this.logger = logger;
        }
        public async Task<IActionResult> OnGet(string returnUrl = null)
        {
            ViewData["IsHomePage"] = false;
            await this.signInManager.SignOutAsync();
            this.logger.LogInformation("Потребителя излезе от акаунта си.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToPage();
            }
        }
    }
}
