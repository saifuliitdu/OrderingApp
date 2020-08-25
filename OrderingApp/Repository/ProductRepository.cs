using OrderingApp.Interfaces;
using OrderingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace OrderingApp.Repository
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(IOrderAppContext context) : base(context)
        {
            
        }

        public void GenerateOrder()
        {
            // logic business
        }
    }
}
