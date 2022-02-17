using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesWebApi.Models;

namespace SalesWebApi.Controllers
{
    [Route("api/[controller]")] // this routes to url
    [ApiController] // identifies correct controller
    public class OrdersController : ControllerBase // must inherit from control base
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }
        // when you create your own methods , you have to make sure the methods are named differently to other methods in our controllers otherwise they wont work
        // PUT: api/Orders/Recalc/5
        [HttpPut("recalc/{orderId}")]
        public async Task<ActionResult> RecalculateOrder(int orderId) {
            var order = await _context.Orders.FindAsync(orderId);

            var sum = order.Orderlines.Sum(x => x.Quantity * x.Price); // sums the quantity with the price and stores it into sum var
            
            order.Total = sum;
            await _context.SaveChangesAsync(); // await stops the whole method until the SaveChanges is completed

            return NotFound();
        }

        // GET: api/Orders
        [HttpGet] // http method used in postman
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders() // methods get marked asynch, return type has to be a task as name suggests
        {
            return await _context.Orders  // await generally goes before reading or updating database
                                    .Include(x => x.Customer) // include( => table) includes the customer table pk to fk in orders
                                    .Include(x => x.Orderlines) // includes orderlines in the search for the Order
                                    .ToListAsync();
                                
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders
                                        .Include(x => x.Customer) // include includes Customer with the result set for order
                                        .Include(x => x.Orderlines) // includes orderlines in the result
                                        .SingleOrDefaultAsync(x => x.Id == id); // single or default async gets the Id from orders and matches it with id

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
                        // interface for returning a error message?
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
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

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
