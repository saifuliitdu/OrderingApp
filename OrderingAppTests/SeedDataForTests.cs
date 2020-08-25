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
    public class SeedDataForTests
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
        //[Test()]
        //public void SeedData()
        //{
        //    var silverGroup = new CustomerGroup("Silver", 10);
        //    var platinumGroup = new CustomerGroup("Platinum", 50);
        //    var goldGroup = new CustomerGroup("Gold", 30);
            
        //    var customer1 = new Customer("Saiful", silverGroup);
        //    var customer2 = new Customer("Riaz", silverGroup);
        //    var customer3 = new Customer("Faisal", silverGroup);
            
        //    var babyToy = new Product("Baby Toy", 100);
        //    var babySoap = new Product("Baby Soap", 80);
        //    var babyFood = new Product("Baby Food", 150);
        //    var babyHygine = new Product("Baby Hygine", 50);
        //    var babyPampas = new Product("Baby Pampas", 120);

        //    _customerGroupRepository.Add(silverGroup);
        //    _customerGroupRepository.Add(goldGroup);
        //    _customerGroupRepository.Add(platinumGroup);

        //    _customerRepository.Add(customer1);
        //    _customerRepository.Add(customer2);
        //    _customerRepository.Add(customer3);

        //    _productRepository.Add(babyToy);
        //    _productRepository.Add(babySoap);
        //    _productRepository.Add(babyFood);
        //    _productRepository.Add(babyHygine);
        //    _productRepository.Add(babyPampas);

        //    bool result = _uow.Commit().Result;


        //    Assert.IsTrue(result);
        //}

    }
}