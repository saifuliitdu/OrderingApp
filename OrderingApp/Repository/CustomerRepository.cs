using OrderingApp.Interfaces;
using OrderingApp.Models;
using ServiceStack;
using System;

namespace OrderingApp.Repository
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        IOrderAppContext _context;
        public CustomerRepository(IOrderAppContext context) : base(context)
        {
            _context = context;
        }
        public Customer GetCustomerById()
        {
            try
            {
                var allCustomers = GetAll().Result;

                return allCustomers.FirstNonDefault();
            }catch(Exception e)
            {
                return null;
            }
        }
    }
}
