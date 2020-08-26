using Microsoft.AspNetCore.Mvc.Rendering;
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

    public class CreateOrderViewModel
    {
        public string OrderId { get; set; }
        public string SelectedCustomerId { get; set; }
        public string SelectedItemId { get; set; }
        
        public double TotalAmount { get; set; }
        public double DiscountAmount { get; set; }
        public double GrandTotalAmount { get; set; }
        public List<SelectListItem> Customers { get; set; }
        public List<SelectListItem> Items { get; set; }
        public List<Product> SelectedItems { get; set; }
        public CreateOrderViewModel()
        {
            SelectedItems = new List<Product>();
        }
        //public List<Customer> CustomerList { get; set; }
    }
}
