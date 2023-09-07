using Kota_Palace_Admin.Data;
using Kota_Palace_Admin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Kota_Palace_Admin.Controllers
{
    public class LoginController : Controller
    {
        private readonly SignInManager<AppUsers> signInManager;
        private readonly AppDBContext context;

        public LoginController(SignInManager<AppUsers> signInManager, AppDBContext context)
        {
            this.signInManager = signInManager;
            this.context = context;
        }
        [HttpPost]
        public IActionResult Logout()
        {
            signInManager.SignOutAsync();
            return Redirect("/");
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
                    //var user = await signInManager.UserManager.FindByEmailAsync(userLogin.Email);
                    /*if (user.UserType == "ADMIN")
                    {  TO BE FIXED  */

                    return RedirectToAction("Index", "Home");
                    // }
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
