
namespace Clothing_Store.Areas.Identity.Pages.Account.Manage
{
    using AspNetCoreHero.ToastNotification.Abstractions;
    using Clothing_Store.Data.Data.Models;
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using System.Security.Claims;

    [IgnoreAntiforgeryToken]
    public class Profile : PageModel
    {
        private readonly UserManager<ApplicationUser> usersManager;
        private readonly Cloudinary cloudinary;
        private readonly INotyfService toast; 
        public Profile(UserManager<ApplicationUser> usersManager, IConfiguration configuration, INotyfService toast)
        {
            this.usersManager = usersManager;
            this.toast = toast;

            Account account = new Account(
                    configuration["CloudinarySettings:Name"],
                    configuration["CloudinarySettings:ApiKey"],
                    configuration["CloudinarySettings:SecretKey"]);

            cloudinary = new Cloudinary(account);

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

            public string ProfileImageUrl { get; set; }
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
                Phone = user.PhoneNumber,
                ProfileImageUrl = user.ProfileImageUrl
                
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

            user.UserName = model.Email;
            user.Email = model.Email;
            await this.usersManager.UpdateAsync(user);

            return Page();
        }
        public async Task<IActionResult> OnPostUploadImageAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();

            var user = await this.GetUserAsync();
            string publicId = "profile" + user.Id;
            int heigth = 441;
            int width = 435;

            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    PublicId = publicId,
                    Transformation = new Transformation()
                    .Height(heigth).Width(width)
                };
                uploadResult = await cloudinary.UploadAsync(uploadParams);
            }

            if (uploadResult.Error != null)
            {
                return BadRequest(uploadResult.Error.Message);
            }

            user.ProfileImageUrl = uploadResult.SecureUri.AbsoluteUri;

            await this.usersManager.UpdateAsync(user);

            return new ContentResult
            {
                Content = "Upload successful",
                StatusCode = 200,
                ContentType = "text/plain"
            };
        }

        private async Task<ApplicationUser> GetUserAsync()
        => await this.usersManager.FindByIdAsync(this.User.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
    }
}
