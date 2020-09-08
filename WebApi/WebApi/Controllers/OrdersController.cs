using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {

        OrdersContext db;
        public OrdersController(OrdersContext context)
        {
            db = context;
            
        }
        // GET: api/Orders
        [HttpGet]
        public IEnumerable<Order> Get()
        {
            var op = db.Orders.Include(o => o.OrderProducts).ThenInclude(p => p.Product).ToList();
            return op;
        }
        // GET: api/Orders/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var op = db.Orders.Include(o => o.OrderProducts).ThenInclude(p => p.Product).FirstOrDefault(x => x.Id == id);
            if (op == null)
                return new ObjectResult(new Order());
            return new ObjectResult(op);
        }
        // POST: api/Orders        
        [HttpPost]
        public IActionResult Post(Order order)
        {
            if (order == null)
            {
                return BadRequest();
            }
            db.Orders.Add(order);
            db.SaveChanges();
            return Ok(order);
        }
        // PUT: api/Orders/5
        [HttpPut]
        public IActionResult Put(Order order)
        {
            if (order == null)
            {
                return BadRequest();
            }
            if (!db.Orders.Any(x => x.Id == order.Id))
            {
                return NotFound();
            }
            db.Update(order);
            db.SaveChanges();
            return Ok(order);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Order order = db.Orders.FirstOrDefault(x => x.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            db.Orders.Remove(order);
            db.SaveChanges();
            return Ok(order);
        }
    }
}
