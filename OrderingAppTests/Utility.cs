using OrderingApp.Interfaces;
using OrderingApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderingAppTests
{
    public static class Utility
    {
        public static IMongoDbSettings GetMongoDbSettings()
        {
            IMongoDbSettings settings = new MongoDbSettings { ConnectionString = "mongodb://localhost:27017", DatabaseName = "Products" };
            return settings;
        }
    }
}
