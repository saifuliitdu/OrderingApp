using NUnit.Framework;
using OrderingApp.Interfaces;
using OrderingApp.Models;
using OrderingAppTests;
using System.Linq;

namespace OrderingApp.Repository.Tests
{
    public class ProductRepositoryTests
    {
        IMongoDbSettings _settings;
        IOrderAppContext _context;
        IUnitOfWork _unitOfWork;
        IProductRepository _productRepository;
        [SetUp]
        public void Setup()
        {
            _settings = Utility.GetMongoDbSettings();
            _context = new OrderAppContext(_settings);
            _unitOfWork = new UnitOfWork(_context);
            _productRepository = new ProductRepository(_context);
        }

        [Test()]
        public void AddProductTest()
        {
            Product babyLotion = new Product("Baby Lotion", 100);
            _productRepository.Add(babyLotion);
            // If everything is ok then:
            _unitOfWork.Commit().Wait();

            // The product will be added only after commit
            var testProduct = _productRepository.GetById(babyLotion.Id);
            Assert.AreEqual(babyLotion.Id, testProduct.Result.Id);
        }
        
        [Test]
        public void UpdateProductTest()
        {
            Product babyDoll = new Product("Baby Doll", 100);
            _productRepository.Add(babyDoll);
            // If everything is ok then:
            _unitOfWork.Commit().Wait();

            // it will be null
            var testProduct = _productRepository.GetById(babyDoll.Id);
            testProduct.Result.Description = babyDoll.Description + " updated";
            _productRepository.Update(testProduct.Result);
            // If everything is ok then:
            _unitOfWork.Commit();
            testProduct = _productRepository.GetById(testProduct.Result.Id);
            Assert.AreEqual(babyDoll.Id, testProduct.Result.Id);
            Assert.AreEqual(testProduct.Result.Description, babyDoll.Description + " updated");
        }

        [Test]
        public void DeleteProductTest()
        {
            Product babyMilk = new Product("Baby Milk", 450);
            _productRepository.Add(babyMilk);
            // If everything is ok then:
            _unitOfWork.Commit().Wait();

            _productRepository.Remove(babyMilk.Id);
            // If everything is ok then:
            _unitOfWork.Commit();
            var testProduct = _productRepository.GetById(babyMilk.Id);
            Assert.IsNull(testProduct.Result);
        }

        [Test]
        public void GetAllProductsTest()
        {
            Product babyOil = new Product("Baby Oil", 60);
            _productRepository.Add(babyOil);
            // If everything is ok then:
            _unitOfWork.Commit().Wait();
            var allProducts = _productRepository.GetAll().Result;

            Assert.IsTrue(allProducts.ToList().Count() > 0);
        }

        [Test]
        public void GetProductTest()
        {
            Product babyDress = new Product("Baby Dress", 990);
            _productRepository.Add(babyDress);
            // If everything is ok then:
            _unitOfWork.Commit().Wait();
            var testProduct = _productRepository.GetById(babyDress.Id);

            Assert.IsNotNull(testProduct.Result);
            Assert.AreEqual(testProduct.Result.Name, babyDress.Name);
        }
    }
}