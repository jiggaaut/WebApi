using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Models
{
    public class OrdersContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }

        public OrdersContext(DbContextOptions<OrdersContext> options) : base(options)
        {
            //Database.EnsureDeleted();
           // Database.EnsureCreated();
            
        }
        public static void Initialize(OrdersContext db)
        {
            //db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            if (db.Orders.Any() || db.Products.Any())
            {
                return;
            }
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderProduct>()
            .HasKey(t => new { t.OrderId, t.ProductId });

            modelBuilder.Entity<OrderProduct>()
                .HasOne(sc => sc.Order)
                .WithMany(s => s.OrderProducts)
                .HasForeignKey(sc => sc.OrderId);

            modelBuilder.Entity<OrderProduct>()
                .HasOne(sc => sc.Product)
                .WithMany(c => c.OrderProducts)
                .HasForeignKey(sc => sc.ProductId);
        }

        public DbSet<OrderProduct> OrderProduct { get; set; }
    }
}
