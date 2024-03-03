namespace Clothing_Store.Areas.Identity.Pages.Account
{
    using Clothing_Store.Data.Data.Models;
    using Clothing_Store.Data.Repositories;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using System.ComponentModel.DataAnnotations;
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<RegisterModel> logger;
        private readonly IRepository<ApplicationUser> usersRepository;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IRepository<ApplicationUser> usersRepository)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.usersRepository = usersRepository;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public class InputModel
        {
            [Required(ErrorMessage = "Имената са задължителни,")]
            public string FullName { get; set; }

            [Required(ErrorMessage = "Телефонният номер е задължителен.")]
            [Phone(ErrorMessage = "Телефонният номер, който избрахте, не е валиден.")]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessage = "Имейлът е задължителен.")]
            [EmailAddress(ErrorMessage = "Имейлът, който избрахте, не е валиден.")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Паролата е задължителна.")]
            public string Password { get; set; }

            [Required(ErrorMessage = "Паролата е задължителна.")]
            [Compare(nameof(Password), ErrorMessage = "Паролата не съвпада.")]
            public string ConfirmPassword { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ViewData["IsHomePage"] = false;
            ReturnUrl = returnUrl;
            ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            ViewData["IsHomePage"] = false;
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                if (this.usersRepository.AllAsNoTracking().Any(x => x.Email.ToLower() == this.Input.Email.ToLower()))
                {
                    this.ModelState.AddModelError(string.Empty, "Имейлът вече е зает.");
                }
                else
                {
                    var user = new ApplicationUser
                    { UserName = this.Input.Email, FullName = this.Input.FullName, Email = this.Input.Email, PhoneNumber = this.Input.PhoneNumber };

                    var result = await this.userManager.CreateAsync(user, Input.Password);

                    if (result.Succeeded)
                    {
                        this.logger.LogInformation("Потребител си създаде акаунт.");

                        await this.signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return Page();
        }
    }
}
