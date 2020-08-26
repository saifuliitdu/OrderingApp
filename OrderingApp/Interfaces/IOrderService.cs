using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderingApp.Interfaces
{
    public interface IOrderService
    {
        double CalculateDiscountAmount(double discountInPercent, double total);
    }
}
