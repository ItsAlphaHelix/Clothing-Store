using AspNetCoreHero.ToastNotification.Extensions;
using Clothing_Store.Data.Data;
using Clothing_Store.Data.Data.Models;
using Clothing_Store.Extensions;
using Clothing_Store.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ClothingStoreContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
    .AddErrorDescriber<IdentityErrorDescriberProvider>()
    .AddEntityFrameworkStores<ClothingStoreContext>();


builder.Services.AddControllersWithViews(
    options =>
    {
        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
    }
);

builder.Services.AddSession(options =>
{
    options.IOTimeout = TimeSpan.FromSeconds(1800);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

StripeConfiguration.ApiKey = builder.Configuration.GetValue<string>("StripeSettings:SecretKey");
builder.Services.AddApplicationServices(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseNotyf();
app.MapRazorPages();

app.Run();
