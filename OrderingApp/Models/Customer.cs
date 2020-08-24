using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderingApp.Models
{
    public class Customer
    {
        public Customer(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }
        public Customer(string name, CustomerGroup group)
        {
            Id = Guid.NewGuid();
            Name = name;
            CustomerGroup = group;
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public CustomerGroup CustomerGroup { get; set; }
    }
}
