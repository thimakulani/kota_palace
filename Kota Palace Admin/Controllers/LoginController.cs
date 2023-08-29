using Kota_Palace_Admin.Data;
using Kota_Palace_Admin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Kota_Palace_Admin.Controllers
{
    public class LoginController : Controller
    {
        private SignInManager<AppUsers> signInManager;
        private AppDBContext context;

        public LoginController(SignInManager<AppUsers> signInManager, AppDBContext context)
        {
            this.signInManager = signInManager;
            this.context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(UserLogin userLogin)
        {
            if (ModelState.IsValid)
            {
                var results = await signInManager.PasswordSignInAsync(userLogin.Email, userLogin.Password, false, false);
                if (results.Succeeded)
                {
                    var user = await signInManager.UserManager.FindByEmailAsync(userLogin.Email);
                    if (user.UserType == "ADMIN")
                    {
                        RedirectToAction("Index", "Dashboard");
                    }
                }
                else
                {
                    ModelState.AddModelError("ERROR", "INVALID EMAIL OR PASSWORD");
                }
            }
            return View(userLogin);
        }
    }
}
