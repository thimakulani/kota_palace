using Microsoft.AspNetCore.Mvc;

namespace Kota_Palace_Admin.Controllers
{
    public class LandingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
