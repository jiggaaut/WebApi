using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string ClientName { get; set; }
        public DateTime Date { get; set; }

        public List<OrderProduct> OrderProducts { get; set; }

        public Order()
        {
            OrderProducts = new List<OrderProduct>();
        }
        /*public List<OrderProduct> OrderProducts { get; set; }
        */
    }
}
