using Microsoft.Extensions.DependencyInjection;
using OrderingApp.Interfaces;
using OrderingApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderingApp.Repository
{
    public static class Utility
    {
        public static IMongoDbSettings InitMongoDbSettings(IServiceProvider services)
        {
            var settings = services.GetRequiredService<IMongoDbSettings>();

            return settings;
        }

        public static IMongoDbSettings GetMongoDbSettings()
        {
            IMongoDbSettings _settings = new MongoDbSettings { ConnectionString = "mongodb://localhost:27017", DatabaseName = "OrderAppDB" };
            return _settings;
        }
    }
}
