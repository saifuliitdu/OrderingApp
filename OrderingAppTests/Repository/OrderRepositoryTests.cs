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
        IMongoContext context;
        IUnitOfWork _uow;
        IOrderRepository _orderRepository;
        IOrderDetailsRepository _orderDetailsRepository;
        ICustomerGroupRepository _customerGroupRepository;
        ICustomerRepository _customerRepository;
        IProductRepository _productRepository;
        [SetUp]
        public void Setup()
        {
            settings = Utility.GetMongoDbSettings();
            context = new MongoContext(settings);
            _uow = new UnitOfWork(context);
            _orderDetailsRepository = new OrderDetailsRepository(context);
            _customerGroupRepository = new CustomerGroupRepository(context);
            _customerRepository = new CustomerRepository(context);
            _productRepository = new ProductRepository(context);
            _orderRepository = new OrderRepository(context, _uow, _orderDetailsRepository);
        }
        [Test()]
        public void PlaceOrderTest()
        {
            var silverGroup = new CustomerGroup("Silver", 10);
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
    }
}