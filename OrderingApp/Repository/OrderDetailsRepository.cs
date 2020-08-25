using OrderingApp.Interfaces;
using OrderingApp.Models;
using OrderingApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace OrderingApp.Repository
{
    public class OrderDetailsRepository : BaseRepository<OrderDetails>, IOrderDetailsRepository
    {
        public OrderDetailsRepository(IOrderAppContext context) : base(context)
        {
            
        }
    }
}
