using OrderingApp.Interfaces;
using OrderingApp.Models;
using ServiceStack;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace OrderingApp.Repository
{
    public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
    {
        IUnitOfWork _uow;
        public PaymentRepository(IOrderAppContext context) : base(context)
        {
            
        }
        public PaymentRepository(IOrderAppContext context, IUnitOfWork uow) : base(context)
        {
            _uow = uow;
        }

        public async Task<bool> MakePayment(Order order)
        {
            Payment payment = new Payment(order, order.GrandTotal, true);
            Add(payment);
            return await _uow.Commit();
        }
        public async Task<Payment> GetPaymentDetails(Order order)
        {
            var allPayments = await GetAll();
            var paymentDetails = allPayments.FirstOrDefault(f => f.Order.Id == order.Id);
            
            return paymentDetails;
        }
    }
}
