using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesWebApi.Models;

namespace SalesWebApi.Controllers
{ // these two attributes modify the whole class
    [Route("api/[controller]")] // route is the url you insert into browser to access the data following, localhost:(ournumber)/
    [ApiController] // specifies what controller it is
    public class CustomersController : ControllerBase // inherited class allows us to work with the models and controllers we create,
                                                      // allows our code to send and receive JSON data
    {
        private readonly AppDbContext _context;

        public CustomersController(AppDbContext context) 
        {
            _context = context;                                                 // gets context value and stores it into _context
        }

        // GET: api/Customers
        [HttpGet]                                                               // this method will only read the database
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()   // gets a collection of customers,
                                                                                // actionresult returns different action responses, if things go wrong
            // async to use asynchronous processing,                            // all of these things are inside of a class named Task, web apps can use Task
            // task must be used to use this return type                        // aynschronous continues running multiple code, so it will run Print method and continue to run i = 1 + 1 at the same time
                                                                                // Synchronous processing is doing one thing at a time, so like int i = 0, print(goes to print method data) i = 1 + 1
        {
            return await _context.Customers.ToListAsync(); // ToListAsynch bring back the list in asynch proc
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id) // gets customer id
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer) // like update
        {
            if (id != customer.Id)
            {
                return BadRequest();
            }

            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // doesnt return anything in POSTMAN
        }

        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer) // like insert
        {
            _context.Customers.Add(customer); // adds customer
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomer", new { id = customer.Id }, customer);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id) // deletes customer
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
