using Clothing_Store.Core.Contracts;
using Clothing_Store.Core.ViewModels.Account;
using Clothing_Store.Data.Data.Models;
using Clothing_Store.Data.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Clothing_Store.Core.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        public AccountService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterViewModel model)
        {
            var user = new ApplicationUser()
            {
                FullName = model.FullName,
                UserName = model.Email,
                PhoneNumber = model.Phone,
                Email = model.Email,
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false);
            }

            return result;
        }

        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            var result = await this.signInManager.PasswordSignInAsync(model.Email, model.Password, false ,false);

            if (result.Succeeded)
            {
                return result;
            }

            return result;
        }

        public async Task LogoutAsync()
        {
            await this.signInManager.SignOutAsync();
        }
    }
}
