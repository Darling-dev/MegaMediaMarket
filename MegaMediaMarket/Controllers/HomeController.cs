using SiteASPCOm.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using SiteASPCOm.Data;
using SiteASPCOm.Models;
using SiteASPCOm.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Localization;

namespace SiteASPCOm.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly StoreDbContext storeDbContext;

        private readonly IStringLocalizer<HomeController> _localizer;

        public HomeController(StoreDbContext storeDbContext, IStringLocalizer<HomeController> localizer)
        {
            this.storeDbContext = storeDbContext;
            _localizer = localizer;
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            /*var localized = _localizer["Greeting"];*/
            var products = await storeDbContext.Products.ToListAsync();
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public Task Search(string searchString)
        {
            throw new NotImplementedException();
        }
    }
}