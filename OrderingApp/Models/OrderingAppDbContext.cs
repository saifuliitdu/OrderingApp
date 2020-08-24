using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using OrderingApp.Interfaces;
using OrderingApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace OrderingApp.Models
{
    public class OrderingAppDbContext
    {
        private MongoClient _mongoClient;
        private IMongoDatabase _mongodb;
       
        public OrderingAppDbContext(IMongoDbSettings settings)
        {
            _mongoClient = new MongoClient(settings.ConnectionString);
            _mongodb = _mongoClient.GetDatabase(settings.DatabaseName);
        }
        public IMongoCollection<TEntity> DbSet<TEntity>() where TEntity : class
        {
            var table = typeof(TEntity).GetCustomAttribute<TableAttribute>(false).Name;
            return Mongodb.GetCollection<TEntity>(table);
        }
        public MongoClient MongodbClient { get { return _mongoClient; } set { } }
        public IMongoDatabase Mongodb { get { return _mongodb; } set { } }
    }
}
