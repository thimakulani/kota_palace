using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Kota_Palace_Admin.Data;
using Kota_Palace_Admin.Models;

namespace Kota_Palace_Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly AppDBContext _context;

        public CartsController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet("cust_cart/{id}")]
        public ActionResult<Cart> GetCart(string id)
        {
            var cart = _context.Cart.Where(x => x.Customer_Id == id).ToList();
            if (cart.Any())
            {
                return NotFound();
            }
            return Ok(cart);
        }

        // GET: api/Carts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cart>>> GetCart()
        {
            var data = await _context.Cart.ToListAsync();
            _context.Dispose();
            return data;
        }

        // GET: api/Carts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cart>> GetCart(int id)
        {
            var cart = await _context.Cart.FindAsync(id);

            if (cart == null)
            {
                return NotFound();
            }

            return cart;
        }

        // PUT: api/Carts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCart(int id, Cart cart)
        {
            if (id != cart.Id)
            {
                return BadRequest();
            }

            _context.Entry(cart).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartExists(id))
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

        // POST: api/Carts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cart>> PostCart(Cart cart)
        {
            _context.Cart.Add(cart);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCart", new { id = cart.Id }, cart);
        }

        [HttpPost("item")]
        public ActionResult PostCartItems(Cart item)
        {
            try
            {
                //var cart = _context.Cart.Where(x => x.Customer_Id == item.Customer_Id).FirstOrDefault();

                //var cart_item = item;
                _context.Cart.Add(item);
                _context.SaveChanges();

                return Ok("Items added to cart");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("item/remove/{id}")]
        public IActionResult DeleteCartItem(int id)
        {
            //var cartItem1 = await _context.CartItem.FindAsync(id);
            var c_i = _context.Cart.Where(x => x.Id == id);
            _context.Remove(c_i);
            _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Carts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart(int id)
        {
            var cart = await _context.Cart.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }

            _context.Cart.Remove(cart);
            await _context.SaveChangesAsync();

            return NoContent();
        }



        private bool CartExists(int id)
        {
            return _context.Cart.Any(e => e.Id == id);
        }
    }

    //public class CartViewModel
    //{
    //    public Cart Cart { get; set; }
    //    public IList<CartItem> Items { get; set; }
    //}
}
