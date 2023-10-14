using Kota_Palace_Admin.Data;
using Kota_Palace_Admin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Kota_Palace_Admin.Controllers
{

    public class ApplicationController : Controller
    {
        private readonly SignInManager<AppUsers> signInManager;
        private readonly AppDBContext context;
        private readonly UserManager<AppUsers> manager;
        public ApplicationController(AppDBContext context, SignInManager<AppUsers> signInManager, UserManager<AppUsers> manager)
        {
            this.context = context;
            this.signInManager = signInManager;
            this.manager = manager;
        }
        // GET: ApplicationController
        /* public ActionResult Index()
         {
             var data = context.Business.Include("");
             return View(data);
         }*/

        public async Task<IActionResult> Index()
        {
            var data = context.Business.Include(b => b.Owner).Where(x => string.IsNullOrEmpty(x.Status));
            return View(await data.ToListAsync());
        }
        public async Task<IActionResult> ActiveBusiness()
        {
            var data = context.Business.Include(b => b.Owner).Where(x => x.Status.ToUpper() == "ACTIVE");
            return View(await data.ToListAsync());
        }
        // GET: ApplicationController/Details/5
        public ActionResult Details(int id)
        {
            var data = context.Business.Include("Owner").Where(x => x.Id == id).FirstOrDefault();
            return View(data);
        }

        public ActionResult Approve(int id)
        {
            var data = context.Business.Find(id);//Include("Owner").Where(x => x.Id == id).FirstOrDefault();
            if (data != null)
            {
                data.Status = "Active";
                context.Business.Update(data);
                context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult Apply()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Apply(ApplicationViewModel applicationViewModel)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(applicationViewModel);
            Debug.WriteLine(json);
            if (ModelState.IsValid)
            {
                var business = applicationViewModel.Business;
                var _user = await signInManager.UserManager.FindByEmailAsync(applicationViewModel.AppUsers.Email);
                if (_user == null)
                {
                    var signUp = applicationViewModel.AppUsers;
                    AppUsers user = new()
                    {
                        Email = signUp.Email,
                        Firstname = signUp.Firstname,
                        Lastname = signUp.Lastname,
                        PhoneNumber = signUp.PhoneNumber,
                        UserType = "OWNER",
                        UserName = signUp.Email,
                    };
                    var results = await manager.CreateAsync(user, signUp.Password);
                    if (results.Succeeded)
                    {
                        var user_data = await signInManager.UserManager.FindByEmailAsync(applicationViewModel.AppUsers.Email);
                        business.OwnerId = user_data.Id;
                        AddBusiness(business);
                    }
                    else
                    {
                        foreach (var item in results.Errors)
                        {
                            ModelState.AddModelError(item.Code, item.Description);
                        }
                    }
                }
                else
                {
                    business.OwnerId = _user.Id;
                    AddBusiness(business);
                }
            }

            return View(applicationViewModel);
        }
        private void AddBusiness(Business business)
        {

            context.Business.Add(business);
            context.SaveChanges();
        }
        // GET: ApplicationController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ApplicationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ApplicationController/Edit/5
        public ActionResult Edit(int id)
        {
            return View(id);
        }

        // POST: ApplicationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ApplicationController/Delete/5
        public ActionResult Delete(int id)
        {
            return View(id);
        }

        // POST: ApplicationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
    public class ApplicationViewModel
    {
        [Key]
        public int Id { get; set; }
        public Business Business { get; set; }
        public UserSignUp AppUsers { get; set; }
    }
}
