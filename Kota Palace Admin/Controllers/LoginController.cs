using Kota_Palace_Admin.Data;
using Kota_Palace_Admin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Versioning;

namespace Kota_Palace_Admin.Controllers
{
    public class LoginController : Controller
    {
        private readonly SignInManager<AppUsers> signInManager;
        private readonly AppDBContext context;
        private readonly UserManager<AppUsers> userManager;

        public LoginController(SignInManager<AppUsers> signInManager, AppDBContext context, UserManager<AppUsers> userManager)
        {
            this.signInManager = signInManager;
            this.context = context;
            this.userManager = userManager;
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
                var user = await userManager.FindByEmailAsync(userLogin.Email);
                if(user == null)
                {
                    ModelState.AddModelError("ERROR", "Username not found");
                    return View(userLogin);
                }
                if(!await userManager.IsInRoleAsync(user,"Admin"))
                {
                    ModelState.AddModelError("ERROR", "Unauthorized");
                    return View(userLogin);
                }
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
