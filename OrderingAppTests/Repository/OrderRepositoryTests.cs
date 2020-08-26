using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using OrderingApp.Interfaces;
using OrderingApp.Models;
using OrderingApp.Repository;
using OrderingApp.Services;
using OrderingApp.ViewModel;
using OrderingAppTests;
using Serilog.Core;
using Serilog.Debugging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderingApp.Repository.Tests
{
    public class OrderRepositoryTests
    {
        IMongoDbSettings settings;
        IOrderAppContext context;
        IUnitOfWork _uow;
        IOrderRepository _orderRepository;
        IOrderDetailsRepository _orderDetailsRepository;
        IGroupRepository _groupRepository;
        ICustomerRepository _customerRepository;
        IProductRepository _productRepository;
        IPaymentRepository _paymentRepository;
        IOrderService _orderService;
        ILogger<IOrderRepository> _logger;
        [SetUp]
        public void Setup()
        {
            _logger = Mock.Of<ILogger<IOrderRepository>>();
            settings = Utility.GetMongoDbSettings();
            context = new OrderAppContext(settings);
            _uow = new UnitOfWork(context);
            _orderDetailsRepository = new OrderDetailsRepository(context);
            _groupRepository = new GroupRepository(context);
            _customerRepository = new CustomerRepository(context);
            _productRepository = new ProductRepository(context);
            _paymentRepository = new PaymentRepository(context);
            _orderRepository = new OrderRepository(context, _uow, _logger);
            _orderService = new OrderService();
        }

        [Test()]
        public void PlaceOrderTest()
        {
            var allCustomers = _customerRepository.GetAll().Result;
            var customer = allCustomers.FirstOrDefault(f=>f.Name.Equals("Saiful"));
            var allGroups = _groupRepository.GetAll().Result;
            var silverGroup = allGroups.FirstOrDefault(x => x.Name.Equals("Silver"));

            var allProducts = _productRepository.GetAll().Result;
            var babyToy = allProducts.FirstOrDefault(f => f.Name.Equals("Baby Toy"));
            var babySoap = allProducts.FirstOrDefault(f => f.Name.Equals("Baby Soap"));

            var items = new List<Product> { babyToy, babySoap };

            Order order = new Order
            {
                Customer = customer,
                Items = items,
                //DiscountAmount = discount,
                //TotalAmount = total,
                //GrandTotalAmount = grandTotal
            };
            order.Total = order.Items.Sum(x => x.Price);
            order.Discount = _orderService.CalculateDiscountAmount(order.Customer.Group.Discount, order.Total);
            order.GrandTotal = order.Total - order.Discount;


            var result = _orderRepository.PlaceOrder(order);

            Assert.IsTrue(result.Result);
        }

        [Test()]
        public void AddItemToOrderTest()
        {
            var allOrders = _orderRepository.GetAll();
            var order = allOrders.Result.FirstOrDefault();
            
            var allProducts = _productRepository.GetAll().Result;
            var babyPampas = allProducts.FirstOrDefault(f => f.Name.Equals("Baby Pampas"));

            var result = _orderRepository.AddItemToOrder(order, babyPampas);

            Assert.IsTrue(result.Result);
        }
        [Test()]
        public void RemoveItemFromOrderTest()
        {
            var allOrders = _orderRepository.GetAll();
            var order = allOrders.Result.FirstOrDefault();

            var allProducts = _productRepository.GetAll().Result;
            var babyPampas = allProducts.FirstOrDefault(f => f.Name.Equals("Baby Toy"));

            var result = _orderRepository.RemoveItemFromOrder(order, babyPampas);

            Assert.IsTrue(result.Result);
        }

        [Test()]
        public void ViewOrderDetailsTest()
        {
            var allOrders = _orderRepository.GetAll();
            var order = allOrders.Result.FirstOrDefault();

            var result = _orderRepository.GetById(order.Id);

            Assert.IsNotNull(result.Result);
        }
    }
}