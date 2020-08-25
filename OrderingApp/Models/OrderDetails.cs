using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderingApp.Models
{
    public class OrderDetails
    {
        public OrderDetails(Product product)
        {
            Id = Guid.NewGuid();
            Product = product;
        }
      
        public Guid Id { get; set; }
        public Product Product { get; set; }
    }
}
