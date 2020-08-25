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
        public OrderRepository(IOrderAppContext context) : base(context)
        {

        }
        public OrderRepository(IOrderAppContext context, IUnitOfWork uow) : base(context)
        {
            _uow = uow;
        }
        public OrderRepository(IOrderAppContext context, IUnitOfWork uow, IPaymentRepository paymentRepository) : base(context)
        {
            _uow = uow;
            _paymentRepository = paymentRepository;
        }

        public async Task<bool> PlaceOrder(OrderViewModel model)
        {
            var total = model.Items.Sum(x => x.Price);
            var discount = (model.Customer.Group.Discount != 0 ? (model.Customer.Group.Discount / 100) : 0) * total;
            var grandTotal = total - discount;
            
            Order order = new Order(model.Customer, model.Items, total, discount, grandTotal);

            Add(order);
            
            return await _uow.Commit();
        }

        public async Task<bool> AddItemToOrder(Order order, Product product)
        {
            order.Items.Add(product);

            Update(order);

            return await _uow.Commit();
        }

        public async Task<bool> RemoveItemFromOrder(Order order, Product product)
        {
            order.Items.Remove(product);

            Update(order);

            return await _uow.Commit();
        }
    }
}
