using OrderingApp.Models;
using OrderingApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderingApp.Repository
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        Task<bool> MakePayment(Order order);
        Task<Payment> GetPaymentDetails(Order order);
    }
}
