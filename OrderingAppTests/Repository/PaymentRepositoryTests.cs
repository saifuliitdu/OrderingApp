using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using OrderingApp.Interfaces;
using OrderingApp.Repository;
using OrderingAppTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderingApp.Repository.Tests
{
    [TestFixture()]
    public class PaymentRepositoryTests
    {
        IMongoDbSettings _settings;
        IOrderAppContext _context;
        IUnitOfWork _unitOfWork;
        IPaymentRepository _paymentRepository;
        IOrderRepository _orderRepository;

        [SetUp]
        public void Setup()
        {
            _settings = Utility.GetMongoDbSettings();
            _context = new OrderAppContext(_settings);
            _unitOfWork = new UnitOfWork(_context);
            _paymentRepository = new PaymentRepository(_context);
            _orderRepository = new OrderRepository(_context, _unitOfWork);
        }

        [Test()]
        public void MakePaymentTest()
        {
            var allOrders = _orderRepository.GetAll();
            var order = allOrders.Result.FirstOrDefault();

            _paymentRepository.MakePayment(order);
            var result = _unitOfWork.Commit();

            Assert.IsTrue(result.Result);
        }
        [Test()]
        public void ViewPaymentDetailsTest()
        {
            var allOrders = _orderRepository.GetAll();
            var order = allOrders.Result.FirstOrDefault();

            var paymentDetails = _paymentRepository.GetPaymentDetails(order);
            
            Assert.IsNotNull(paymentDetails.Result);
        }
    }
}