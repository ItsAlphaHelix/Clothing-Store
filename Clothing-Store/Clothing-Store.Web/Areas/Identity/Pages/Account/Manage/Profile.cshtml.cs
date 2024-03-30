
namespace Clothing_Store.Areas.Identity.Pages.Account.Manage
{
    using Clothing_Store.Data.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using System.Security.Claims;

    [IgnoreAntiforgeryToken]
    public class Profile : PageModel
    {
        private readonly UserManager<ApplicationUser> usersManager;

        public Profile(UserManager<ApplicationUser> usersManager)
        {
            this.usersManager = usersManager;
        }

        [BindProperty]
        public ProfileViewModel ProfileModel { get; set; }

        //public string ReturnUrl { get; set; }
        public class ProfileViewModel
        {
            public string FirstName { get; set; }

            public string LastName { get; set; }

            public string Email { get; set; }

            public string Phone { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["IsHomePage"] = false;

            var user = await this.usersManager.GetUserAsync(User);

            ProfileModel = new ProfileViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.PhoneNumber
            };

            return Page();
        }

        public async Task<IActionResult> OnPostEditPhoneNumberAsync(ProfileViewModel model)
        {
            ViewData["IsHomePage"] = false;
            var user = await this.usersManager.FindByIdAsync(this.User.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);


            user.PhoneNumber = model.Phone;
            await this.usersManager.UpdateAsync(user);

            return Page();
        }

        public async Task<IActionResult> OnPostEditEmailAddressAsync(ProfileViewModel model)
        {
            ViewData["IsHomePage"] = false;
            var user = await this.usersManager.FindByIdAsync(this.User.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);


            user.Email = model.Email;
            await this.usersManager.UpdateAsync(user);

            return Page();
        }
    }
}
