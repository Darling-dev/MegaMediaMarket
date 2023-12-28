using Microsoft.AspNetCore.Mvc;

namespace SiteASPCOm.Controllers
{
    public class CatalogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
