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
        public Customer(string name, Group group)
        {
            Id = Guid.NewGuid();
            Name = name;
            Group = group;
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Group Group { get; set; }
    }
}
