using NUnit.Framework;
using OrderingApp.Interfaces;
using OrderingApp.Models;
using OrderingApp.Repository;
using OrderingApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderingApp.Repository.Tests
{
    public class ProductRepositoryTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test()]
        public void AddProductTest()
        {

            MongoDbSettings settings = new MongoDbSettings { ConnectionString = "mongodb://localhost:27017", DatabaseName = "Products" };
            IMongoContext context = new MongoContext(settings);
            IProductRepository _productRepository = new ProductRepository(context);
            IUnitOfWork _uow = new UnitOfWork(context);
            Product product = new Product("test");
            _productRepository.Add(product);
            // it will be null
            var testProduct = _productRepository.GetById(product.Id);

            // If everything is ok then:
            _uow.Commit();

            // The product will be added only after commit
            testProduct = _productRepository.GetById(product.Id);
            Assert.AreEqual(product.Id, testProduct.Result.Id);
        }

        [Test]
        public void UpdateProductTest()
        {

            MongoDbSettings settings = new MongoDbSettings { ConnectionString = "mongodb://localhost:27017", DatabaseName = "Products" };
            IMongoContext context = new MongoContext(settings);
            IProductRepository _productRepository = new ProductRepository(context);
            IUnitOfWork _uow = new UnitOfWork(context);
            Product product = new Product("test");
            _productRepository.Add(product);
            // If everything is ok then:
             _uow.Commit().Wait();
            //await b.Result;
            // it will be null
            var testProduct = _productRepository.GetById(product.Id);
            testProduct.Result.Description = "updated";
            _productRepository.Update(testProduct.Result);
            // If everything is ok then:
            _uow.Commit();
            testProduct = _productRepository.GetById(testProduct.Result.Id);
            Assert.AreEqual(product.Id, testProduct.Result.Id);
        }

        [Test]
        public void DeleteProductTest()
        {
            MongoDbSettings settings = new MongoDbSettings { ConnectionString = "mongodb://localhost:27017", DatabaseName = "Products" };
            IMongoContext context = new MongoContext(settings);
            IProductRepository _productRepository = new ProductRepository(context);
            IUnitOfWork _uow = new UnitOfWork(context);
            Product product = new Product("test");
            _productRepository.Add(product);
            // If everything is ok then:
            _uow.Commit().Wait();
           
            _productRepository.Remove(product.Id);
            // If everything is ok then:
            _uow.Commit();
            var testProduct = _productRepository.GetById(product.Id);
            Assert.IsNull(testProduct.Result);
        }

        [Test]
        public void GetAllProductsTest()
        {
            MongoDbSettings settings = new MongoDbSettings { ConnectionString = "mongodb://localhost:27017", DatabaseName = "Products" };
            IMongoContext context = new MongoContext(settings);
            IProductRepository _productRepository = new ProductRepository(context);
            IUnitOfWork _uow = new UnitOfWork(context);
            var allProducts = _productRepository.GetAll().Result;
           
            Assert.IsTrue(allProducts.ToList().Count() > 0);
        }

        [Test]
        public void GetProductTest()
        {
            MongoDbSettings settings = new MongoDbSettings { ConnectionString = "mongodb://localhost:27017", DatabaseName = "Products" };
            IMongoContext context = new MongoContext(settings);
            IProductRepository _productRepository = new ProductRepository(context);
            IUnitOfWork _uow = new UnitOfWork(context);
            Product product = new Product("test");
            _productRepository.Add(product);
            // If everything is ok then:
            _uow.Commit().Wait();
            var getProduct = _productRepository.GetById(product.Id);

            Assert.IsNotNull(getProduct.Result);
            Assert.AreEqual(getProduct.Result.Description, product.Description);
        }
    }
}