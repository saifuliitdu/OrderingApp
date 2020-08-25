using OrderingApp.Interfaces;
using OrderingApp.Models;

namespace OrderingApp.Repository
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(IMongoContext context) : base(context)
        {
            
        }
    }
}
