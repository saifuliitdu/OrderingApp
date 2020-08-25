using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderingApp.Models
{
    public class Order
    {
        public Order(Customer customer, ICollection<Product> items, double total, double discount, double grandTotal)
        {
            Id = Guid.NewGuid();
            Customer = customer;
            //OrderDetails = orderDetails;
            Items = items;
            Total = total;
            Discount = discount;
            GrandTotal = grandTotal;
        }
        public Guid Id { get; set; }
        public double Total { get; set; }
        public double Discount { get; set; }
        public double GrandTotal { get; set; }
        public Customer Customer { get; set; }
        public ICollection<Product> Items { get; set; }
        //public ICollection<OrderDetails> OrderDetails { get; set; }
    }
}
