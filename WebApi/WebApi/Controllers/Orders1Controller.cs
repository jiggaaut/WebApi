using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using WebApi.ViewModels;

namespace WebApi.Controllers
{
    public class Orders1Controller : Controller
    {
        private readonly OrdersContext _context;

        public Orders1Controller(OrdersContext context)
        {
            _context = context;
        }

        // GET: Orders1
        public IActionResult Index()
        {
            List<Order> orders = _context.Orders.Include(o => o.OrderProducts).ThenInclude(p => p.Product).ToList();
            List<Product> products = _context.Products.ToList();
            IndexViewModel ivm = new IndexViewModel { Orders = orders, Products = products };
            return View(ivm);
        }

        // GET: Orders1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.Include(o => o.OrderProducts).ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders1/Create
        public IActionResult Create()
        {
            ViewBag.Products = _context.Products.ToList();
            return View();
        }

        // POST: Orders1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create(Order order, int[] selectedProducts)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                Order newOrder =_context.Orders.FirstOrDefault(x => x.Equals(order));
                if (selectedProducts != null)
                {
                    for (int i = 0; i < selectedProducts.Length; i++)
                    {
                        _context.OrderProduct.Add(new OrderProduct { OrderId = newOrder.Id, ProductId = selectedProducts[i] });
                        _context.SaveChanges();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Orders1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Products = _context.Products.ToList();
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.Include(o => o.OrderProducts).ThenInclude(p => p.Product).FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Orders1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Order order, int[] selectedProducts)
        {
            if (id != order.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    IQueryable<OrderProduct> op = _context.OrderProduct.Where(x => x.OrderId==order.Id);
                    if (op!=null)
                    {
                        _context.OrderProduct.RemoveRange(op);
                        _context.SaveChanges();
                    }
                    if (selectedProducts != null)
                    {
                        for (int i = 0; i < selectedProducts.Length; i++)
                        {
                            _context.OrderProduct.Add(new OrderProduct { OrderId = order.Id, ProductId = selectedProducts[i] });
                            _context.SaveChanges();
                        }            
                    }
                    _context.Entry(order).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Orders1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
