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
        IMongoDbSettings _settings;
        IOrderAppContext _context;
        IUnitOfWork _unitOfWork;
        IOrderRepository _orderRepository;
        IGroupRepository _groupRepository;
        ICustomerRepository _customerRepository;
        IProductRepository _productRepository;
        IPaymentRepository _paymentRepository;
        IOrderService _orderService;
        [SetUp]
        public void Setup()
        {
            _settings = Utility.GetMongoDbSettings();
            _context = new OrderAppContext(_settings);
            _unitOfWork = new UnitOfWork(_context);
            _groupRepository = new GroupRepository(_context);
            _customerRepository = new CustomerRepository(_context);
            _productRepository = new ProductRepository(_context);
            _paymentRepository = new PaymentRepository(_context);
            _orderRepository = new OrderRepository(_context, _unitOfWork);
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

            //var allProducts = _productRepository.GetAll().Result;
            var babyPampas = order.Items.FirstOrDefault();//allProducts.FirstOrDefault(f => f.Name.Equals("Baby Toy"));

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