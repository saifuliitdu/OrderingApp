using NUnit.Framework;
using OrderingApp.Interfaces;
using OrderingApp.Models;
using OrderingApp.Repository;
using OrderingApp.ViewModel;
using OrderingAppTests;
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
        IGroupRepository _customerGroupRepository;
        ICustomerRepository _customerRepository;
        IProductRepository _productRepository;
        IPaymentRepository _paymentRepository;
        [SetUp]
        public void Setup()
        {
            settings = Utility.GetMongoDbSettings();
            context = new OrderAppContext(settings);
            _uow = new UnitOfWork(context);
            _orderDetailsRepository = new OrderDetailsRepository(context);
            _customerGroupRepository = new CustomerGroupRepository(context);
            _customerRepository = new CustomerRepository(context);
            _productRepository = new ProductRepository(context);
            _paymentRepository = new PaymentRepository(context);
            _orderRepository = new OrderRepository(context, _uow, _paymentRepository);
        }
        [Test()]
        public void PlaceOrderTest()
        {
            var silverGroup = new Group("Silver", 10);
            var items = new List<Product> { new Product("Baby Toy", 100), new Product("Baby Shop", 80) };

            OrderViewModel orderViewModel = new OrderViewModel
            {
                Customer = new Customer("Saiful", silverGroup),
                Items = items,
                //DiscountAmount = discount,
                //TotalAmount = total,
                //GrandTotalAmount = grandTotal
            };
            var result = _orderRepository.PlaceOrder(orderViewModel);

            Assert.IsTrue(result.Result);
        }

        [Test()]
        public void PlaceOrderTest2()
        {
            var allCustomers = _customerRepository.GetAll().Result;
            var customer = allCustomers.FirstOrDefault(f=>f.Name.Equals("Saiful"));
            var allGroups = _customerGroupRepository.GetAll().Result;
            var silverGroup = allGroups.FirstOrDefault(x => x.Name.Equals("Silver"));

            var allProducts = _productRepository.GetAll().Result;
            var babyToy = allProducts.FirstOrDefault(f => f.Name.Equals("Baby Toy"));
            var babySoap = allProducts.FirstOrDefault(f => f.Name.Equals("Baby Soap"));

            var items = new List<Product> { babyToy, babySoap };

            OrderViewModel orderViewModel = new OrderViewModel
            {
                Customer = customer,
                Items = items,
                //DiscountAmount = discount,
                //TotalAmount = total,
                //GrandTotalAmount = grandTotal
            };
            var result = _orderRepository.PlaceOrder(orderViewModel);

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