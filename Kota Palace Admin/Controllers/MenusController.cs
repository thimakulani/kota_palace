using System;
using System.Collections.Generic;
using System.Linq;
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
    public class MenusController : ControllerBase
    {
        private readonly AppDBContext _context;

        public MenusController(AppDBContext context)
        {
            _context = context;
        }

        // GET: api/Menus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Menu>>> GetMenu()
        {
            return await _context.Menu.ToListAsync();
        }

        // GET: api/Menus/5
        [HttpGet("single/{id}")]
        public ActionResult<Menu> GetMenu(int id)
        {
            var menu = _context.Menu.Where(x => x.BusinessId == id).FirstOrDefault();

            if (menu == null)
            {
                return NotFound();
            }

            return Ok(menu);
        }
        [HttpGet("all/{id}")]
        public ActionResult<ICollection<Menu>> GetAllMenu(int id)
        {
            var menu = _context.Menu.Where(x => x.BusinessId == id).Include("Extras").ToList();

            if (menu == null)
            {
                return NotFound();
            }

            return Ok(menu);
        }

        // PUT: api/Menus/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMenu(int id, Menu menu)
        {
            if (id != menu.Id)
            {
                return BadRequest();
            }

            _context.Entry(menu).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(menu);
        }

        // POST: api/Menus
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Menu>> PostMenu(Menu menu)
        {
            _context.Menu.Add(menu);
            await _context.SaveChangesAsync();

            return Ok("Menu was successfully added!!!");
        }

        // DELETE: api/Menus/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenu(int id)
        {
            var menu = await _context.Menu.FindAsync(id);
            if (menu == null)
            {
                return NotFound();
            }

            _context.Menu.Remove(menu);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MenuExists(int id)
        {
            return _context.Menu.Any(e => e.Id == id);
        }
    }
}
