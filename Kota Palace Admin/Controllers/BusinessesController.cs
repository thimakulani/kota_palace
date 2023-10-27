using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Kota_Palace_Admin.Models;
using Kota_Palace_Admin.Data;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace Kota_Palace_Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessesController : ControllerBase
    {
        private readonly AppDBContext _context;
        private readonly SignInManager<AppUsers> signInManager;
        private readonly UserManager<AppUsers> manager;
        public BusinessesController(AppDBContext context, SignInManager<AppUsers> signInManager, UserManager<AppUsers> manager)
        {
            _context = context;
            this.signInManager = signInManager;
            this.manager = manager;
        }

        // GET: api/Businesses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Business>>> GetBusiness()
        {
            return await _context.Business.Where(x=>x.Status == "Active").ToListAsync();
        }

        // GET: api/Businesses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Business>> GetBusiness(int id)
        {
            var business = await _context.Business.FindAsync(id);

            if (business == null)
            {
                return NotFound();
            }

            return business;
        }
        //
        [HttpGet("specific/{id}")]
        public ActionResult<Business> GetBusiness(string id)
        {
            var business = _context.Business.Where(x => x.OwnerId == id).Include(x => x.Address).FirstOrDefault();

            if (business == null)
            {
                return NotFound("Not found");
            }

            return Ok(business);
        }

        // PUT: api/Businesses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBusiness(int id, Business business)
        {
            if (id != business.Id)
            {
                return BadRequest();
            }

            _context.Entry(business).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BusinessExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        [HttpGet("online/{id}")]
        public async Task<IActionResult> UpdateOnline(int id) 
        {
            var busness = await _context.Business.FindAsync(id);
            if(busness!=null)
            {
                if(busness.Online == "ONLINE")
                {
                    busness.Online = "OFFLINE";
                }
                else
                {
                    busness.Online = "ONLINE";
                }
                _context.Business.Update(busness);
                _context.SaveChanges();
                return Ok(busness);
            }
            return NotFound("Something went wrong");
        }



        // PUT: api/Businesses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("address")]
        public IActionResult PostAddress(Address address)
        {
            //var result = _context.Business.Where(x => x.OwnerId == business.OwnerId).FirstOrDefault();
            if (address != null)
            {
                _context.Addresses.Add(address);
                _context.SaveChanges();
                return Ok();
            }
            return BadRequest("Something went wrong adding address");
        }
        [HttpGet("test")]
        public ActionResult<ApplicationViewModel> Test()
        {
            UserSignUp userSignUp = new()
            {
                Email = "",
                Firstname = "",
                Lastname = "",
                Id = "",
                Password = "",
                PhoneNumber = ""
            };
            Business business = new()
            {
                Name = "",
                PhoneNumber = "",
                Description = "",

            };
            return Ok(new ApplicationViewModel() { AppUsers = userSignUp, Business = business });
        }

        // POST: api/Businesses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("register")]
        public async Task<ActionResult<ApplicationViewModel>> PostBusiness(ApplicationViewModel applicationViewModel)
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
                    string errors = "";
                    foreach (var item in results.Errors)
                    {
                        //ModelState.AddModelError(item.Code, item.Description);
                        errors += item.Description;
                    }
                    return NotFound(errors);
                }
            }
            else
            {
                business.OwnerId = _user.Id;
                AddBusiness(business);
            }

            return Ok("Your business hass been successfully created");
        }

        private void AddBusiness(Business business)
        {
            _context.Business.Add(business);
            _context.SaveChanges();
        }

        // DELETE: api/Businesses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBusiness(int id)
        {
            var business = await _context.Business.FindAsync(id);
            if (business == null)
            {
                return NotFound();
            }

            _context.Business.Remove(business);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpGet("buss_menu/{id}")]
        public ActionResult<BusinessMenuViewModel> GetBussMenu(int id)
        {
            var bus_menu = new BusinessMenuViewModel()
            {
                Business = _context.Business.Find(id),
                Menu = _context.Menu.Where(x => x.BusinessId == id).Include("Extras").ToList(),
            };
            return bus_menu;
        }

        private bool BusinessExists(int id)
        {
            return _context.Business.Any(e => e.Id == id);
        }
    }
    public class BusinessMenuViewModel
    {
        public Business Business { get; set; }
        public List<Menu> Menu { get; set; }
    }
}
