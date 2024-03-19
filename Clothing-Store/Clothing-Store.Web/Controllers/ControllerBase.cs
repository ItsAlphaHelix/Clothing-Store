namespace Clothing_Store.Controllers
{
    using Clothing_Store.Core.Contracts;
    using Clothing_Store.Core.Services;
    using Clothing_Store.Data.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;
    public class ControllerBase : Controller
    {
        private readonly UserManager<ApplicationUser> usersManager;
        private readonly IBagsService bagsService;
        public ControllerBase(UserManager<ApplicationUser> usersManager, IBagsService bagsService)
        {
            this.usersManager = usersManager;
            this.bagsService = bagsService;
        }

        protected async Task<ApplicationUser> GetUserAsync()
           => await this.usersManager.FindByIdAsync(this.User.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

        /// <summary>
        /// The method verifies the user's existence.
        /// If the user exists, it returns their ID; otherwise, it generates a temporary user ID, adds it to a cookie, and returns the generated ID.
        /// </summary>
        /// <returns></returns>
        protected async Task<string> GetUserIdAsync()
        {
            string temporaryUserId = bagsService.GetOrCreateTemporaryUserId();
            var user = await GetUserAsync();
            string userId = string.Empty;

            if (user == null)
            {
                userId = temporaryUserId;
            }
            else
            {
                userId = user.Id;
            }

            return userId;
        }

    }
}
