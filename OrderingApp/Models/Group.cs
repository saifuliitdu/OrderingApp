using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderingApp.Models
{
    public class Group
    {
        public Group(string name, double discount = 0)
        {
            Id = Guid.NewGuid();
            Name = name;
            Discount = discount;
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Discount { get; set; }
    }
}
