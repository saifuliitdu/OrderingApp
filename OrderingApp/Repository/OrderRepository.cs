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
        IOrderDetailsRepository _orderDetailsRepository;
        public OrderRepository(IMongoContext context) : base(context)
        {

        }
        public OrderRepository(IMongoContext context, IUnitOfWork uow) : base(context)
        {
            _uow = uow;
        }
        public OrderRepository(IMongoContext context, IUnitOfWork uow, IOrderDetailsRepository orderDetailsRepository) : base(context)
        {
            _uow = uow;
            _orderDetailsRepository = orderDetailsRepository;
        }

        public async Task<bool> PlaceOrder(OrderViewModel model)
        {
            var total = model.Items.Sum(x => x.Price);
            var discount = (model.Customer.CustomerGroup.Discount != 0 ? (model.Customer.CustomerGroup.Discount / 100) : 0) * total;
            var grandTotal = total - discount;
            List<OrderDetails> orderDetailList = new List<OrderDetails>();

            model.Items.ToList().ForEach(x=> {
                OrderDetails orderDetails = new OrderDetails(x);
                orderDetailList.Add(orderDetails);
            });

            Order order = new Order(model.Customer, orderDetailList, total, discount, grandTotal);

            Add(order);
            //await _uow.Commit();

            //foreach(var orderDetail in order.OrderDetails)
            //{
            //    orderDetail.Order = order;
            //    _orderDetailsRepository.Update(orderDetail);
            //}
            

            return await _uow.Commit();
        }
    }
}
