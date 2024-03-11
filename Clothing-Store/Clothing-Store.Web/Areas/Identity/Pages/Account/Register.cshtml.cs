namespace Clothing_Store.Areas.Identity.Pages.Account
{
    using Clothing_Store.Data.Data.Models;
    using Clothing_Store.Data.Repositories;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;

    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<RegisterModel> logger;
        private readonly IRepository<ApplicationUser> usersRepository;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IRepository<Customer> customersRepository;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IRepository<ApplicationUser> usersRepository,
            IHttpContextAccessor httpContextAccessor,
            IRepository<Customer> customersRepository)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.usersRepository = usersRepository;
            this.httpContextAccessor = httpContextAccessor;
            this.customersRepository = customersRepository;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public class InputModel
        {
            [Required(ErrorMessage = "Името е задължително")]
            [StringLength(30, MinimumLength = 3, ErrorMessage = "Името трябва да бъде с дължина между 3 и 30 символа.")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Фамилията е задължителна")]
            [StringLength(30, MinimumLength = 3, ErrorMessage = "Фамилията трябва да бъде с дължина между 3 и 30 символа.")]
            public string LastName { get; set; }

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
                    var httpContext = this.httpContextAccessor.HttpContext;

                    var user = new ApplicationUser
                    { UserName = this.Input.Email, FirstName = this.Input.FirstName, LastName = this.Input.LastName, Email = this.Input.Email, PhoneNumber = this.Input.PhoneNumber };

                    if (httpContext.Request.Cookies.ContainsKey("TemporaryUserId"))
                    {
                        string userId = httpContext.Request.Cookies["TemporaryUserId"];
                        user.Id = userId;

                        httpContext.Response.Cookies.Delete("TemporaryUserId");

                        var customer = await customersRepository.All()
                            .FirstOrDefaultAsync(x => x.CustomerId == userId);

                        customer ??= new Customer();

                        customer.FirstName = user.FirstName;
                        customer.LastName = user.LastName;
                        customer.Phone = user.PhoneNumber;
                        customer.Email = user.Email;

                        await customersRepository.SaveChangesAsync();
                    }

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
