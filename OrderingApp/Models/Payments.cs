using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderingApp.Models
{
    public class Payments
    {
        public Payments(Order order, double amount, bool isPaid, string type = "")
        {
            Id = Guid.NewGuid();
            Order = order;
            Amount = amount;
            IsPaid = isPaid;
            Type = type;
        }
        public Guid Id { get; set; }
        public Order Order { get; set; }
        public double Amount { get; set; }
        public bool IsPaid { get; set; }
        public string Type { get; set; }
    }
}
