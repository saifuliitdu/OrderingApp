﻿using NUnit.Framework;
using OrderingApp.Interfaces;
using OrderingApp.Models;
using System.Linq;

namespace OrderingApp.Repository.Tests
{
    public class ProductRepositoryTests
    {
        MongoDbSettings settings;
        IMongoContext context;
        IUnitOfWork _uow;
        IProductRepository _productRepository;
        [SetUp]
        public void Setup()
        {
            settings = new MongoDbSettings { ConnectionString = "mongodb://localhost:27017", DatabaseName = "Products" };
            context = new MongoContext(settings);
            _uow = new UnitOfWork(context);
            _productRepository = new ProductRepository(context);
        }

        [Test()]
        public void AddProductTest()
        {
            Product product = new Product("test");
            _productRepository.Add(product);
            // If everything is ok then:
            _uow.Commit().Wait();

            // The product will be added only after commit
            var testProduct = _productRepository.GetById(product.Id);
            Assert.AreEqual(product.Id, testProduct.Result.Id);
        }

        [Test]
        public void UpdateProductTest()
        {
            Product product = new Product("test");
            _productRepository.Add(product);
            // If everything is ok then:
            _uow.Commit().Wait();

            // it will be null
            var testProduct = _productRepository.GetById(product.Id);
            testProduct.Result.Description = product.Description + " updated";
            _productRepository.Update(testProduct.Result);
            // If everything is ok then:
            _uow.Commit();
            testProduct = _productRepository.GetById(testProduct.Result.Id);
            Assert.AreEqual(product.Id, testProduct.Result.Id);
        }

        [Test]
        public void DeleteProductTest()
        {
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
            Product product = new Product("test");
            _productRepository.Add(product);
            // If everything is ok then:
            _uow.Commit().Wait();
            var allProducts = _productRepository.GetAll().Result;

            Assert.IsTrue(allProducts.ToList().Count() > 0);
        }

        [Test]
        public void GetProductTest()
        {
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