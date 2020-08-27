using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderingApp.Models
{
    public class Order
    {
        public Order() { Id = Guid.NewGuid(); Items = new List<Product>(); }
        public Order(Customer customer, ICollection<Product> items, double total, double discount, double grandTotal)
        {
            Id = Guid.NewGuid();
            Customer = customer;
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
        public Payment Payment { get; set; }
        public ICollection<Product> Items { get; set; }
    }
}
