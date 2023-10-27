using Google.Api;
using Kota_Palace_Admin.Data;
using Kota_Palace_Admin.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Kota_Palace_Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, AppDBContext context)
        {
            _logger = logger;
            _context = context;
        }
        private AppDBContext _context;
        public IActionResult Index()
        {
            _context.Business.Where(x => x.Status.ToUpper() == "ACTIVE");
            HomeViewModel homeViewModel = new()
            {
                PendingBusinessCount = _context.Business.Where(x => x.Status.ToUpper() == "ACTIVE").ToList().Count,
                ActiveBusinessCount = _context.Business.Where(x => x.Status.ToUpper() != "ACTIVE").ToList().Count,
                UsersCount = _context.Users.Count(),
            };

            return View(homeViewModel);
        }
        public IActionResult Login()
        {
            return View();
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
    }
    public class HomeViewModel
    {
        public int PendingBusinessCount { get; set; }
        public int ActiveBusinessCount { get; set; }  
        public int UsersCount { get; set; }  

    }
}