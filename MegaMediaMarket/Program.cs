using SiteASPCOm.Models.Domain;
using SiteASPCOm.Repositories.Abstract;
using SiteASPCOm.Repositories.Implementation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SiteASPCOm.Data;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<StoreDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MvcDemoConnectionString")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<StoreDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options => options.LoginPath = "/UserAuthentication/Login");

builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ShoppingCartService>();

builder.Services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { new CultureInfo("en-US"), new CultureInfo("uk-UA") };
    options.DefaultRequestCulture = new RequestCulture("en-US");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseRequestLocalization();


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "home",
        pattern: "/",
        defaults: new { controller = "Home", action = "Index" });

    endpoints.MapControllerRoute(
        name: "login",
        pattern: "/login/",
        defaults: new { controller = "UserAuthentication", action = "Login" });

    endpoints.MapControllerRoute(
        name: "register",
        pattern: "/register/",
        defaults: new { controller = "UserAuthentication", action = "Registration" });

    
    endpoints.MapControllerRoute(
        name: "changepasword",
        pattern: "/Dashboard/ChangePassword",
        defaults: new { controller = "UserAuthentication", action = "ChangePassword" });


    endpoints.MapControllerRoute(
        name: "view",
        pattern: "/Products/View",
        defaults: new { controller = "Products", action = "View" });

    endpoints.MapControllerRoute(
        name: "catalog",
        pattern: "/catalog/",
        defaults: new { controller = "Products", action = "Index" });

    endpoints.MapControllerRoute(
        name: "faq",
        pattern: "/faq/",
        defaults: new { controller = "FAQ", action = "Index" });

    endpoints.MapDefaultControllerRoute();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Products}/{action=Index}/{id?}");

app.Run();
