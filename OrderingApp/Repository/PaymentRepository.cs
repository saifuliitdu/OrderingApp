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
        IUnitOfWork _unitOfWork;
        public PaymentRepository(IOrderAppContext context) : base(context)
        {
            
        }
        public PaymentRepository(IOrderAppContext context, IUnitOfWork unitOfWork) : base(context)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> MakePayment(Order order)
        {
            Payment payment = new Payment(order, order.GrandTotal, true);
            Add(payment);
            return await _unitOfWork.Commit();
        }
        public async Task<Payment> GetPaymentDetails(Order order)
        {
            var allPayments = await GetAll();
            var paymentDetails = allPayments.FirstOrDefault(f => f.Order.Id == order.Id);
            
            return paymentDetails;
        }
    }
}
