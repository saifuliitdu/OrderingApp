using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderingApp.Models
{
    public class Order
    {
        public Order(Customer customer, ICollection<OrderDetails> orderDetails, double total, double discount, double grandTotal)
        {
            Id = Guid.NewGuid();
            Customer = customer;
            OrderDetails = orderDetails;
            Total = total;
            Discount = discount;
            GrandTotal = grandTotal;
        }
        public Guid Id { get; set; }
        public double Total { get; set; }
        public double Discount { get; set; }
        public double GrandTotal { get; set; }
        public Customer Customer { get; set; }
        public ICollection<OrderDetails> OrderDetails { get; set; }
    }
}
