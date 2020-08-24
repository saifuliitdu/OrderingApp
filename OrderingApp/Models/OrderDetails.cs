using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderingApp.Models
{
    public class OrderDetails
    {
        public OrderDetails(Product product, Order order)
        {
            Id = Guid.NewGuid();
            Product = product;
            Order = order;
        }
        public Guid Id { get; set; }
        public Product Product { get; set; }
        public Order Order { get; set; }
    }
}
