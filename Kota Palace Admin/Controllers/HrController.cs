using Kota_Palace_Admin.Data;
using Kota_Palace_Admin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;
using System.Linq;

namespace Kota_Palace_Admin.Controllers
{
    public class HrController : Controller
    {
        private readonly AppDBContext _context;
        private readonly UserManager<AppUsers> manager;
        public HrController(AppDBContext context, UserManager<AppUsers> manager)
        {
            _context = context;
            this.manager = manager;
        }

        public IActionResult Index()
        {
            var users = manager.Users.ToList();
            // var users = _context.Users.ToList<AppUsers>();

            return View(users); // Assuming you have a view named 'Index'
        }
        public IActionResult Create()
        {

            // var users = _context.Users.ToList<AppUsers>();

            return View(); // Assuming you have a view named 'Index'
        }
        [HttpPost]
        public async Task<IActionResult> Create(UserSignUp userSignUp)
        {
            if (ModelState.IsValid)
            {
                var app_users = new AppUsers()
                {
                    Firstname = userSignUp.Firstname,
                    Lastname = userSignUp.Lastname,
                    PhoneNumber = userSignUp.PhoneNumber,
                    UserName = userSignUp.Email,
                    UserType = "ADMIN",
                };
                var results = await manager.CreateAsync(app_users, userSignUp.Password);

                if (results.Succeeded)
                {
                    return RedirectToAction("Index", "Hr");
                }

                string error = "";

                foreach (var res in results.Errors)
                {
                    error = $"{error} {res.Description}\n";
                }

                ModelState.AddModelError(string.Empty, error);

            }
            //var users = manager.Users.ToList();
            // var users = _context.Users.ToList<AppUsers>();

            return View(userSignUp); // Assuming you have a view named 'Index'
        }

    }
}
