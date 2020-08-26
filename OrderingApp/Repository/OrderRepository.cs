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
        IUnitOfWork _uow;
        IPaymentRepository _paymentRepository;
        IOrderService _orderService;
        ILogger<IOrderRepository> _logger;
        public OrderRepository(IOrderAppContext context, ILogger<IOrderRepository> logger) : base(context)
        {
            _logger = logger;
        }
        public OrderRepository(IOrderAppContext context, IUnitOfWork uow, ILogger<IOrderRepository> logger) : base(context)
        {
            _uow = uow;
            _logger = logger;
        }
        public OrderRepository(IOrderAppContext context, IUnitOfWork uow, IPaymentRepository paymentRepository, ILogger<IOrderRepository> logger) : base(context)
        {
            _uow = uow;
            _paymentRepository = paymentRepository;
            _logger = logger;
        }

        public async Task<bool> PlaceOrder(Order order)
        {
            try
            {
                Add(order);
                var commitResult = await _uow.Commit();

                // log
                string message = string.Format("Success: Customer: {0} successfully place order, order id: ", order.Customer.Name, order.Id);
                _logger.LogWarning(message);

                return commitResult;
            }
            catch(Exception e)
            {
                string message = string.Format("Customer: {0} try to place order.  Exception: {1}", order.Customer.Name, e.Message);
                _logger.LogWarning(message);
                throw;
            }
        }

        public async Task<bool> AddItemToOrder(Order order, Product product)
        {
            try
            {
                order.Items.Add(product);

                Update(order);
                var commitResult = await _uow.Commit();

                // log
                string message = string.Format("Success: Customer: {0} successfully add product name: {1} into order id: {2}", order.Customer.Name, product.Name, order.Id);
                _logger.LogWarning(message);

                return commitResult;
            }
            catch(Exception e)
            {
                string message = string.Format("Customer: {0} try to add item name: {1} into order id: {2}  Exception: {3}", order.Customer.Name, product.Name, order.Id, e.Message);
                _logger.LogWarning(message);
                throw;
            }
        }

        public async Task<bool> RemoveItemFromOrder(Order order, Product product)
        {
            try
            {
                order.Items.Remove(product);

                Update(order);

                var commitResult = await _uow.Commit();

                // log
                string message = string.Format("Success: Customer: {0} successfully remove product name: {1} from order id: {2}", order.Customer.Name, product.Name, order.Id);
                _logger.LogWarning(message);

                return commitResult;
            }
            catch(Exception e)
            {
                string message = string.Format("Customer: {0} try to remove item name: {1} from order id: {2} Exception: {3}", order.Customer.Name, product.Name, order.Id, e.Message);
                _logger.LogWarning(message);
                throw;
            }
        }
    }
}
