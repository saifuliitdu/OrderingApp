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
        public double Discount { get; set; }
        public double TotalAmount { get; set; }
        public double DiscountAmount { get; set; }
        public double GrandTotalAmount { get; set; }
        public List<SelectListItem> Customers { get; set; }
        public List<SelectListItem> Items { get; set; }
        public List<Product> SelectedItems { get; set; }
        public bool IsPaid { get; set; }
        public CreateOrderViewModel()
        {
            SelectedItems = new List<Product>();
            CustomerList = new List<Customer>();
        }
        public List<Customer> CustomerList { get; set; }
    }

    public class PaymentViewModel
    {
        public Customer Customer { get; set; }
        public string OrderId { get; set; }
        public double Discount { get; set; }
        public double TotalAmount { get; set; }
        public double DiscountAmount { get; set; }
        public double GrandTotalAmount { get; set; }
        public List<Product> SelectedItems { get; set; }
        public string PaymentStatus { get; set; }
        public PaymentViewModel()
        {
            SelectedItems = new List<Product>();
        }
    }
    public class RemoveItemFromOrderViewModel
    {
        public string OrderId { get; set; }
        public string ProductId { get; set; }
    }
}
