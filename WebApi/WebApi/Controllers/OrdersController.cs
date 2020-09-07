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
            if (!db.Orders.Any())
            {
                Product p1 = new Product { Name = "Intel Core I5 4590", Price = 300 };
                Product p2 = new Product { Name = "Radeon R9 280", Price = 250 };
                Product p3 = new Product { Name = "Hynix 8GB", Price = 125 };
                Product p4 = new Product { Name = "Intel Core I7 6700HQ", Price = 500 };
                Product p5 = new Product { Name = "Nvidia 960M", Price = 346 };
                Product p6 = new Product { Name = "WD Blue 256GB", Price = 425 };
                Product p7 = new Product { Name = "Ryzen 2600", Price = 175 };
                Product p8 = new Product { Name = "Nvidia GTX 1660", Price = 321 };
                Product p9 = new Product { Name = "Samsung EVO 960", Price = 960 };
                db.Products.AddRange(new List<Product> { p1, p2, p3, p4, p5, p6, p7, p8, p9 });
                Order o1 = new Order { ClientName = "Jiggaaut", Date = new DateTime(2020, 5, 5) };
                Order o2 = new Order { ClientName = "OnceDoter", Date = new DateTime(2020, 6, 6) };
                Order o3 = new Order { ClientName = "Mortagriwa", Date = DateTime.Now };

                db.Orders.AddRange(new List<Order> { o1, o2, o3 });
                db.SaveChanges();

                o1.OrderProducts.Add(new OrderProduct { OrderId = o1.Id, ProductId = p1.Id });
                o1.OrderProducts.Add(new OrderProduct { OrderId = o1.Id, ProductId = p2.Id });
                o1.OrderProducts.Add(new OrderProduct { OrderId = o1.Id, ProductId = p3.Id });
                o2.OrderProducts.Add(new OrderProduct { OrderId = o2.Id, ProductId = p4.Id });
                o2.OrderProducts.Add(new OrderProduct { OrderId = o2.Id, ProductId = p5.Id });
                o2.OrderProducts.Add(new OrderProduct { OrderId = o2.Id, ProductId = p6.Id });
                o3.OrderProducts.Add(new OrderProduct { OrderId = o3.Id, ProductId = p7.Id });
                o3.OrderProducts.Add(new OrderProduct { OrderId = o3.Id, ProductId = p8.Id });
                o3.OrderProducts.Add(new OrderProduct { OrderId = o3.Id, ProductId = p9.Id });
                db.SaveChanges();
            }
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
        public Order Get(int id)
        {
            var op = db.Orders.Include(o => o.OrderProducts).ThenInclude(p => p.Product).FirstOrDefault(x => x.Id == id);
            if (op == null)
                return new Order();
            return op;
        }
        // POST: api/Orders        
        [HttpPost]
        public ActionResult Post(Order order)
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
        public ActionResult Put(Order order)
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
        public ActionResult Delete(int id)
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
