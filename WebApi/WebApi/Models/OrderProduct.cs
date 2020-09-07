using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class OrderProduct
    {
        //public int Id { get; set; }
        [JsonIgnore]
        public int OrderId { get; set; }
        public Order Order { get; set; }
        [JsonIgnore]
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
