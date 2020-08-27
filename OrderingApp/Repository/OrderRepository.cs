using DnsClient.Internal;
using Microsoft.Extensions.Logging;
using OrderingApp.Interfaces;
using OrderingApp.Models;
using OrderingApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace OrderingApp.Repository
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        IUnitOfWork _unitOfWork;
        IPaymentRepository _paymentRepository;
        IOrderService _orderService;
        public OrderRepository(IOrderAppContext context) : base(context)
        {

        }
        public OrderRepository(IOrderAppContext context, IUnitOfWork unitOfWork) : base(context)
        {
            _unitOfWork = unitOfWork;
        }
        public OrderRepository(IOrderAppContext context, IUnitOfWork unitOfWork, IPaymentRepository paymentRepository) : base(context)
        {
            _unitOfWork = unitOfWork;
            _paymentRepository = paymentRepository;
        }

        public async Task<IEnumerable<Order>> GetAllOrders()
        {

            try
            {
                var allOrders = GetAll().Result;
                var allPayments = _paymentRepository.GetAll().Result;

                allOrders.ToList().ForEach(x => {

                    x.Payment = allPayments.FirstOrDefault(f => f.Order.Id == x.Id);
                });


                return allOrders;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<bool> PlaceOrder(Order order)
        {
            try
            {
                Add(order);
                var commitResult = await _unitOfWork.Commit();

                return commitResult;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<bool> AddItemToOrder(Order order, Product product)
        {
            try
            {
                order.Items.Add(product);

                Update(order);
                var commitResult = await _unitOfWork.Commit();

                return commitResult;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<bool> RemoveItemFromOrder(Order order, Product product)
        {
            try
            {
                order.Items.Remove(product);

                Update(order);

                var commitResult = await _unitOfWork.Commit();

                return commitResult;
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
