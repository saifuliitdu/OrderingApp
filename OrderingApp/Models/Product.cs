using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderingApp.Models
{
    public class Product
    {
        public Product(string name)
        {
            Name = name;
            Id = Guid.NewGuid();
        }
        public Product(string name, double price, string description = "")
        {
            Id = Guid.NewGuid();
            Description = description;
            Name = name;
            Price = price;
        }
        public Product(Guid id, string name, double price, string description = "")
        {
            Id = id;
            Description = description;
            Name = name;
            Price = price;
        }

        public Guid Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
    }
}
