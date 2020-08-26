using Microsoft.Extensions.Logging;
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
        IMongoDbSettings _settings;
        IOrderAppContext _context;
        IUnitOfWork _unitOfWork;
        IOrderRepository _orderRepository;
        IGroupRepository _groupRepository;
        ICustomerRepository _customerRepository;
        IProductRepository _productRepository;
        IPaymentRepository _paymentRepository;

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
        }
        [Test()]
        public void SeedData()
        {
            var silverGroup = new Group("Silver", 10);
            var platinumGroup = new Group("Platinum", 50);
            var goldGroup = new Group("Gold", 30);

            var customerOne = new Customer("Saiful", silverGroup);
            var customerTwo = new Customer("Riaz", platinumGroup);
            var customerThree = new Customer("Faisal", goldGroup);
            
            var babyToy = new Product("Baby Toy", 100);
            var babySoap = new Product("Baby Soap", 80);
            var babyFood = new Product("Baby Food", 150);
            var babyHygine = new Product("Baby Hygine", 50);
            var babyPampas = new Product("Baby Pampas", 120);

            _groupRepository.Add(silverGroup);
            _groupRepository.Add(goldGroup);
            _groupRepository.Add(platinumGroup);

            _customerRepository.Add(customerOne);
            _customerRepository.Add(customerTwo);
            _customerRepository.Add(customerThree);

            _productRepository.Add(babyToy);
            _productRepository.Add(babySoap);
            _productRepository.Add(babyFood);
            _productRepository.Add(babyHygine);
            _productRepository.Add(babyPampas);

            bool result = _unitOfWork.Commit().Result;


            Assert.IsTrue(result);
        }

    }
}