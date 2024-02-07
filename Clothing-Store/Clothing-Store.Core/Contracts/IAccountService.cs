namespace Clothing_Store.Core.Contracts
{
    using Clothing_Store.Core.ViewModels.Account;
    using Microsoft.AspNetCore.Identity;
    public interface IAccountService
    {
        Task<IdentityResult> RegisterAsync(RegisterViewModel model);

        Task<SignInResult> LoginAsync(LoginViewModel model);

        Task LogoutAsync();
    }
}
