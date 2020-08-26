using OrderingApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderingApp.Services
{
    public class OrderService  : IOrderService
    {
        public double CalculateDiscountAmount(double discountInPercent, double total)
        {
            double discountAmount = (discountInPercent != 0 ? (discountInPercent / 100) : 0) * total;
            return discountAmount;
        }
    }
}
