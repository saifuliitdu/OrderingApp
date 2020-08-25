using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderingApp.Models
{
    public class Payment
    {
        public Payment(Order order, double paymentAmount, bool isPaid)
        {
            Id = Guid.NewGuid();
            Order = order;
            PaymentAmount = paymentAmount;
            IsPaid = isPaid;
        }
        public Guid Id { get; set; }
        public Order Order { get; set; }
        public double PaymentAmount { get; set; }
        public bool IsPaid { get; set; }
    }
}
