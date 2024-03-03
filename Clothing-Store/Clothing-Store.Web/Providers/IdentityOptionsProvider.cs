namespace Clothing_Store.Providers
{
    using Microsoft.AspNetCore.Identity;
    public static class IdentityOptionsProvider
    {
        public static void GetIdentityOptions(IdentityOptions options)
        {
            options.Password.RequiredLength = 8;
            options.Password.RequireDigit = true;
        }
    }
}
