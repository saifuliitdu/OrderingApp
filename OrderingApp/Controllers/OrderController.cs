using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DnsClient.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using OrderingApp.Interfaces;
using OrderingApp.Models;
using OrderingApp.Repository;
using OrderingApp.ViewModel;

namespace OrderingApp.Controllers
{
    public class OrderController : Controller
    {
        IOrderRepository _orderRepository;
        IUnitOfWork _unitOfWork;
        IProductRepository _productRepository;
        ICustomerRepository _customerRepository;
        IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;
        static CreateOrderViewModel CurrentOrder;
        public OrderController(IOrderRepository orderRepository, IUnitOfWork unitOfWork, IProductRepository productRepository, ICustomerRepository customerRepository, IOrderService orderService, ILogger<OrderController> logger)
        {
            _logger = logger;
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
            _orderService = orderService;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("The order page has been accessed");
            var allOrders = _orderRepository.GetAll().Result;

            return View(allOrders);
        }
        public IActionResult Create()
        {
            var allProducts = _productRepository.GetAll().Result;
            var allCustomers = _customerRepository.GetAll().Result;
            CreateOrderViewModel Order;
            //if (CurrentOrder == null)
            //{
            //if (string.IsNullOrEmpty(orderId))
            //{
                Order = new CreateOrderViewModel
                {
                    //OrderId = Guid.NewGuid().ToString(),
                    Customers = GetCustomerItemList(allCustomers),
                    Items = GetProductItemList(allProducts),
                    //CustomerList = allCustomers.ToList(),
                    SelectedItems = new List<Product>()
                };
            //}
            //else
            //{
            //    var o = _orderRepository.GetById(Guid.Parse(orderId));
            //    Order = new CreateOrderViewModel
            //    {
            //        //OrderId = Guid.NewGuid().ToString(),
            //        Customers = GetCustomerItemList(allCustomers),
            //        Items = GetProductItemList(allProducts),
            //        //CustomerList = allCustomers.ToList(),
            //        SelectedItems = o.Result.Items.ToList(),
            //        TotalAmount = o.Result.Total,
            //        DiscountAmount = o.Result.Discount,
            //        GrandTotalAmount = o.Result.GrandTotal
            //    };
            //}
             
            //}
            return View(Order);
        }
       
        //[HttpPost]
        ////[ValidateAntiForgeryToken]
        //public IActionResult Create(CreateOrderViewModel order)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        //Order

        //        //_orderRepository.Add(order);
        //        //_unitOfWork.Commit();
        //        //return RedirectToAction("Index");
        //    }
        //    return View(order);
        //}

        [HttpPost]
        public IActionResult AddToOrder(CreateOrderViewModel order)
        {
            Order orderToBeSaved = null;
            if (string.IsNullOrEmpty(order.OrderId))
            {
                orderToBeSaved = new Order();
                orderToBeSaved.Items = new List<Product>();
            }
            else
            {
                orderToBeSaved = _orderRepository.GetById(Guid.Parse(order.OrderId)).Result;
            }
           
            if (ModelState.IsValid)
            {
                
                //var existingOrder = _orderRepository.GetById(Guid.Parse(order.OrderId)).Result;
                var existingProduct = _productRepository.GetById(Guid.Parse(order.SelectedItemId)).Result;
                var existingCustomer = _customerRepository.GetById(Guid.Parse(order.SelectedCustomerId)).Result;
                var obj = _customerRepository.GetCustomerById();
                var total = order.TotalAmount + existingProduct.Price;
                var discountAmount = _orderService.CalculateDiscountAmount(existingCustomer.Group.Discount, total);
                var grandTotal = total - discountAmount;
                orderToBeSaved.Total += total;
                orderToBeSaved.Discount += discountAmount;
                orderToBeSaved.GrandTotal += grandTotal;
                orderToBeSaved.Items.Add(existingProduct);
                orderToBeSaved.Customer = existingCustomer;

                if (string.IsNullOrEmpty(order.OrderId))
                    _orderRepository.Add(orderToBeSaved);
                else
                    _orderRepository.Update(orderToBeSaved);
                _unitOfWork.Commit().Wait();
                //CurrentOrder.TotalAmount += total;
                //CurrentOrder.DiscountAmount += discountAmount;
                //CurrentOrder.GrandTotalAmount += grandTotal;
                //CurrentOrder.SelectedItems.Add(existingProduct);
                //CurrentOrder.SelectedCustomerId = order.SelectedCustomerId;

                order.OrderId = orderToBeSaved.Id.ToString();
                order.SelectedItems = orderToBeSaved.Items.ToList();
                order.TotalAmount = orderToBeSaved.Total;
                order.DiscountAmount = orderToBeSaved.Discount;
                order.GrandTotalAmount = orderToBeSaved.GrandTotal;

                var allProducts = _productRepository.GetAll().Result;
                var allCustomers = _customerRepository.GetAll().Result;
                order.Customers = GetCustomerItemList(allCustomers);
                order.Items = GetProductItemList(allProducts);
            }
            return View("Create", order);
        }

        #region Dropdownlist 

        private List<SelectListItem> GetProductItemList(IEnumerable<Product> allProducts)
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            allProducts.ToList().ForEach(x =>
            {
                var productItem = new SelectListItem { Text = x.Name, Value = x.Id.ToString() };
                selectListItems.Add(productItem);
            });

            return selectListItems;
        }
        private List<SelectListItem> GetCustomerItemList(IEnumerable<Customer> allCustomers)
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            allCustomers.ToList().ForEach(x =>
            {
                var customerItem = new SelectListItem { Text = x.Name, Value = x.Id.ToString() };
                selectListItems.Add(customerItem);
            });

            return selectListItems;
        }
        #endregion


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
