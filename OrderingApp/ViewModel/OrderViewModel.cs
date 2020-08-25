using OrderingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderingApp.ViewModel
{
    public class OrderViewModel
    {
        public Customer Customer { get; set; }
        public ICollection<Product> Items { get; set; }
        public double TotalAmount { get; set; }
        public double DiscountAmount { get; set; }
        public double GrandTotalAmount { get; set; }
    }
}
