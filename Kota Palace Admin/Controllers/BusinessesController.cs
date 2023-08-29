using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Kota_Palace_Admin.Models;
using Kota_Palace_Admin.Data;

namespace Kota_Palace_Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessesController : ControllerBase
    {
        private readonly AppDBContext _context;

        public BusinessesController(AppDBContext context)
        {
            _context = context;
        }

        // GET: api/Businesses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Business>>> GetBusiness()
        {
            return await _context.Business.ToListAsync();
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
            var business = _context.Business.Where(x => x.OwnerId == id).FirstOrDefault();

            if (business == null)
            {
                return NotFound();
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

        // POST: api/Businesses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("register")]
        public async Task<ActionResult<Business>> PostBusiness(Business business)
        {
            var result = _context.Business.Where(x => x.OwnerId == business.OwnerId).FirstOrDefault();
            if (result == null)
            {
                _context.Business.Add(business);
                await _context.SaveChangesAsync();
            }
            else
            {
                //
                _context.Business.Update(business);
                await _context.SaveChangesAsync();
            }

            return Ok(business);
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
}
public class BusinessMenuViewModel
{
    public Business Business { get; set; }
    public List<Menu> Menu { get; set; }
}