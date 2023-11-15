using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Kota_Palace_Admin.Models;
using Kota_Palace_Admin.Data;
using Microsoft.AspNetCore.SignalR;
using Kota_Palace_Admin.Hubs;
using FirebaseAdmin.Messaging;

namespace Kota_Palace_Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly AppDBContext _context;
        private IHubContext<OrderHub> hubContext;
        private IWebHostEnvironment webHostEnvironment;
        public OrdersController(AppDBContext context, IHubContext<OrderHub> hubContext, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.hubContext = hubContext;
            this.webHostEnvironment = webHostEnvironment;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrder()
        {
            return await _context.Order.ToListAsync();
        }

        // GET: api/Orders/5
        [HttpGet("{business_id}")]
        public ActionResult<IEnumerable<Order>> GetOrder(int business_id)
        {
            var order = _context.Order.Where(x => x.BusinessId == business_id && x.Status == "Pending").Include(x => x.OrderItems).Include(x => x.Customer);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        [HttpGet("single/{id}")]
        public ActionResult<Order> GetSingleOrder(int id)
        {
            var order = _context.Order.Where(x => x.Id == id).Include(x => x.OrderItems).Include(x => x.Customer).FirstOrDefault();

            if (order == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(order);
            }
        }

        [HttpGet("customer/{id}")]
        public ActionResult<IEnumerable<Order>> GetCustomerOrder(string id)
        {
            var order = _context.Order.Where(x => x.CustomerId == id && x.Status == "Pending" || x.Status == "Accepted").Include(x => x.OrderItems);
            //var order = await _context.Order.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        [HttpGet("status/{id}")]
        public async Task<ActionResult<Order>> GetOrderAsync(int id)
        {
            var order = await _context.Order.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }


            return Ok(order);
        }

        //To get the orders with completed status
        [HttpGet("completed/{id}")]
        public ActionResult<IEnumerable<Order>> GetCompletedOrder(int id)
        {
            var order = _context.Order.Where(x => x.Status == "Complete" && x.BusinessId == id).Include(x => x.Customer).Include(x => x.OrderItems);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }
        [HttpGet("prepare/{id}")]
        public ActionResult<IEnumerable<Order>> GetPreparedOrder(int id)
        {
            var order = _context.Order.Where(x => x.Status == "Accepted" && x.BusinessId == id).Include(x => x.Customer).Include(x => x.OrderItems);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        //To get the orders with completed status
        [HttpGet("customer/completed/{id}")]
        public ActionResult<IEnumerable<Order>> GetCompletedCustomerOrders(string id)
        {
            var order = _context.Order.Where(x => x.Status == "Complete" && x.CustomerId == id).Include(x => x.OrderItems);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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

        //update order status to in-progress
        [HttpPut("update_status/{id}")]
        public async Task<IActionResult> PutProcessOrder(int id, Order order)
        {

            if (id != order.Id)
            {
                return BadRequest();
            }

            var processOrder = await _context.Order.FindAsync(order.Id);

            if (processOrder == null)
            {
                return NotFound("Order not found");
            }

            if (processOrder.Status == "Pending")
            {
                processOrder.Status = "Accepted";
            }
            else if (processOrder.Status == "Accepted")
            {
                processOrder.Status = "Completed";
            }
            await _context.SaveChangesAsync();
            //var _order = _context.Order.Where(x => x.Id == order.Id).Include(x=>x.Customer).Include(x => x.OrderItems).FirstOrDefault();
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(processOrder); // Serialize the updated processOrder
            await hubContext.Clients.All.SendAsync(processOrder.CustomerId, json);
            var fb_admin = FirestoreInstance
                .GetFirebaseMessaging(webHostEnvironment);
            Message message = new()
            {
                Topic = $"{processOrder.CustomerId}",
                Notification = new Notification()
                {
                    Title = "ORDER UPDATE",
                    Body = $"Your order status has been changed to: {processOrder.Status}"
                },
            };
            await fb_admin.SendAsync(message);

            AppLogs appLogs = new()
            {
                Message = processOrder.CustomerId
            };
            _context.AppLogs.Add(appLogs);
            _context.SaveChanges();
            Console.WriteLine(json);
            return Ok(processOrder.Status);

        }
        [HttpGet("test")]
        public async Task<ActionResult> Test(string id)
        {

            // Update records in the database
            /*var orders = _context.Order.Where(x => x.Status == "Accepted").ToList();
            for (int i = 0; i < orders.Count; i++)
            {
                orders[i].Status = "Pending";
            }*/
            var list = _context.Order.Include(x => x.OrderItems);
            _context.RemoveRange(list);
            _context.SaveChanges();

            await hubContext.Clients.All.SendAsync(id, "data");
            return Ok();

        }


        //update order status to in-progress
        [HttpPut("complete/{id}")]
        public async Task<IActionResult> PutCompleteOrder(int id, Order order)
        {
            order = new Order()
            {
                Id = id,
                Status = "Ready"
            };

            if (id != order.Id)
            {
                return BadRequest();
            }

            _context.Order.Attach(order);
            var update = _context.Entry(order);
            update.Property(x => x.Status).IsModified = true;

            await _context.SaveChangesAsync();

            return Ok($"Order - {id} has been completed");
        }

        //update order status to in-progress
        [HttpPut("assign/driver/{id}")]
        public async Task<IActionResult> PutAssignDriverToOrder(int id, Order order)
        {
            order = new Order()
            {
                Id = id,
                Status = "Ready"
            };

            if (id != order.Id)
            {
                return BadRequest();
            }

            _context.Order.Attach(order);
            var update = _context.Entry(order);
            update.Property(x => x.Status).IsModified = true;

            await _context.SaveChangesAsync();

            return Ok($"Order - {id} has been completed");
        }

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("order")]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            _context.Order.Add(order);
            try
            {
                await _context.SaveChangesAsync();
                //_context.Order.Remove(order);
                var data = _context.Cart.Where(x => x.Customer_Id == order.CustomerId).ToList();
                _context.RemoveRange(data);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            order.Customer = _context.AppUsers.Find(order.CustomerId);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(order);
            await hubContext.Clients.All.SendAsync(order.BusinessId.ToString(), json);

            return Ok("Order has been placed!!!");
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(string id)
        {
            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Order.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.Id == id);
        }
    }
}
