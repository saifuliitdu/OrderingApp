using OrderingApp.Interfaces;
using OrderingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderingApp.Repository
{
    public class SeedData
    {
        IMongoDbSettings _settings;
        IOrderAppContext _context;
        IUnitOfWork _unitOfWork;
        IOrderRepository _orderRepository;
        IGroupRepository _groupRepository;
        ICustomerRepository _customerRepository;
        IProductRepository _productRepository;
        IPaymentRepository _paymentRepository;
        public  SeedData(IMongoDbSettings settings)
        {
            _settings = settings;
            _context = new OrderAppContext(_settings);
            _unitOfWork = new UnitOfWork(_context);
            _groupRepository = new GroupRepository(_context);
            _customerRepository = new CustomerRepository(_context);
            _productRepository = new ProductRepository(_context);
            _paymentRepository = new PaymentRepository(_context);
            _orderRepository = new OrderRepository(_context, _unitOfWork);
        }
        public void InitSeedData()
        {
            var isDataExists = _groupRepository.GetAll().Result;

            if (isDataExists.Count() > 0) return;

            var silverGroup = new Group("Silver", 10);
            var platinumGroup = new Group("Platinum", 30);
            var goldGroup = new Group("Gold", 20);

            var customerOne = new Customer("Mr. Saiful", silverGroup);
            var customerTwo = new Customer("Mr. Riaz", platinumGroup);
            var customerThree = new Customer("Mrs. Anika", goldGroup);
            var customerFour = new Customer("Mr. Selim", silverGroup);
            var customerFive = new Customer("Mr. Talha", goldGroup);

            var babyToy = new Product("Baby Toy", 350);
            var babySoap = new Product("Baby Soap", 80);
            var babyFood = new Product("Baby Food", 150);
            var babyHygine = new Product("Baby Hygine", 50);
            var babyPampas = new Product("Baby Pampas", 120);
            var babyDoll = new Product("Baby Doll", 560);
            var babyLotion = new Product("Baby Lotion", 260);
            var book = new Product("Book", 230);
            var pen = new Product("Pen", 50);
            var paper = new Product("Paper", 140);

            _groupRepository.Add(silverGroup);
            _groupRepository.Add(goldGroup);
            _groupRepository.Add(platinumGroup);

            _customerRepository.Add(customerOne);
            _customerRepository.Add(customerTwo);
            _customerRepository.Add(customerThree);
            _customerRepository.Add(customerFour);
            _customerRepository.Add(customerFive);

            _productRepository.Add(babyToy);
            _productRepository.Add(babySoap);
            _productRepository.Add(babyFood);
            _productRepository.Add(babyHygine);
            _productRepository.Add(babyPampas);
            _productRepository.Add(babyDoll);
            _productRepository.Add(babyLotion);
            _productRepository.Add(book);
            _productRepository.Add(pen);
            _productRepository.Add(paper);

            _unitOfWork.Commit().Wait();
        }
    }
}
