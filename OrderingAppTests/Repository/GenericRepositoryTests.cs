using NUnit.Framework;
using OrderingApp.Interfaces;
using OrderingApp.Models;
using OrderingApp.Repository;
using OrderingApp.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderingApp.Repository.Tests
{
    public class GenericRepositoryTests
    {
        IGenericRepository<Book> genericRepository;
        [SetUp]
        public void Setup()
        {
            IMongoDbSettings settings = new MongoDbSettings
            {
                ConnectionString = "mongodb://localhost:27017",
                DatabaseName = "OrderAppDb",
                CollectionName = "Orders"
            };
            OrderingAppDbContext orderDbContext = new OrderingAppDbContext(settings);
            genericRepository = new GenericRepository<Book>(orderDbContext);
        }


        [Test()]
        public void FindTest()
        {
            Book book = new Book
            {
                Name = "Book 1",
                Price = 100,
                Category = "Category 1",
                Author = "Author 1"
            };
            var b = genericRepository.Create(book);
            
            var findBook = genericRepository.Find(( b.Result as Book).Id);

            Assert.IsNotNull(findBook);
            Assert.AreEqual((b.Result as Book).Name, findBook.Result.Name);
            
        }

        [Test()]
        public void UpdateTest()
        {
            Book book = new Book
            {
                Name = "Book 1",
                Price = 100,
                Category = "Category 1",
                Author = "Author 1"
            };
            var b = genericRepository.Create(book);
            book.Name = book.Name + " updated";
            var updated = genericRepository.Update(book);
            //var updatedObj = genericRepository.Find(book.Id);
            Assert.IsTrue(updated.Result);
            Assert.AreEqual((b.Result as Book).Id, book.Id);
        }

        [Test()]
        public void CreateTest()
        {
            Book book = new Book
            {
                Name = "Book 1",
                Price = 100,
                Category = "Category 1",
                Author = "Author 1"
            };
            var b = genericRepository.Create(book);
            Assert.IsNotNull(b.Result);
            Assert.AreEqual((b.Result as Book).Name, book.Name);
        }

        [Test()]
        public void DeleteTest()
        {
            Book book = new Book
            {
                Name = "Book 1",
                Price = 100,
                Category = "Category 1",
                Author = "Author 1"
            };
            genericRepository.Create(book);
            var deleted = genericRepository.Delete(book.Id);
            var deletedObj = genericRepository.Find(book.Id);
            Assert.IsTrue(deleted.Result);
        }

        //[Test()]
        //public void QueryTest()
        //{
        //    Assert.Fail();
        //}

        //[Test()]
        //public void QueryTest1()
        //{
        //    Assert.Fail();
        //}

        //[Test()]
        //public void GetTest()
        //{


        //    Assert.Fail();
        //}

    }
}