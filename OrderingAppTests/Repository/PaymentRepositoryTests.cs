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
        IMongoDbSettings settings;
        IOrderAppContext context;
        IUnitOfWork _uow;
        IPaymentRepository _paymentRepository;
        IOrderRepository _orderRepository;

        [SetUp]
        public void Setup()
        {
            settings = Utility.GetMongoDbSettings();
            context = new OrderAppContext(settings);
            _uow = new UnitOfWork(context);
            _paymentRepository = new PaymentRepository(context);
            _orderRepository = new OrderRepository(context, _uow);
        }

        [Test()]
        public void MakePaymentTest()
        {
            var allOrders = _orderRepository.GetAll();
            var order = allOrders.Result.FirstOrDefault();

            _paymentRepository.MakePayment(order);
            var result = _uow.Commit();

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