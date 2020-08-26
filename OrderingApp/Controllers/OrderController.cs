using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DnsClient.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using OrderingApp.Interfaces;
using OrderingApp.Models;
using OrderingApp.Repository;
using OrderingApp.ViewModel;
using ServiceStack;

namespace OrderingApp.Controllers
{
    public class OrderController : Controller
    {
        IOrderRepository _orderRepository;
        IUnitOfWork _unitOfWork;
        IProductRepository _productRepository;
        ICustomerRepository _customerRepository;
        IOrderService _orderService;
        IPaymentRepository _paymentRepository;
        private readonly ILogger<OrderController> _logger;
        public OrderController(IOrderRepository orderRepository, IUnitOfWork unitOfWork, IProductRepository productRepository, ICustomerRepository customerRepository, IOrderService orderService, IPaymentRepository paymentRepository, ILogger<OrderController> logger)
        {
            _logger = logger;
            _paymentRepository = paymentRepository;
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

            Order = new CreateOrderViewModel
            {
                Customers = GetCustomerItemList(allCustomers),
                Items = GetProductItemList(allProducts),
                SelectedItems = new List<Product>()
            };

            return View(Order);
        }

        [HttpPost]
        public IActionResult AddItemToOrder(CreateOrderViewModel order)
        {

            if (ModelState.IsValid)
            {
                Order orderToBeSaved;
                if (string.IsNullOrEmpty(order.OrderId))
                {
                    orderToBeSaved = new Order();
                    orderToBeSaved.Items = new List<Product>();
                }
                else
                {
                    orderToBeSaved = _orderRepository.GetById(Guid.Parse(order.OrderId)).Result;
                }


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

                order.OrderId = orderToBeSaved.Id.ToString();
                order.SelectedItems = orderToBeSaved.Items.ToList();
                order.TotalAmount = orderToBeSaved.Total;
                order.DiscountAmount = orderToBeSaved.Discount;
                order.GrandTotalAmount = orderToBeSaved.GrandTotal;
                order.Discount = existingCustomer.Group.Discount;

                var allProducts = _productRepository.GetAll().Result;
                var allCustomers = _customerRepository.GetAll().Result;
                order.Customers = GetCustomerItemList(allCustomers);
                order.Items = GetProductItemList(allProducts);
            }
            return View("Create", order);
        }


        public IActionResult RemoveItemFromOrder(string orderId, string itemId)
        {
            //var existingProduct = _productRepository.GetById(Guid.Parse(itemId)).Result;
            var existingOrder = _orderRepository.GetById(Guid.Parse(orderId)).Result;
            var existingProduct = existingOrder.Items.FirstOrDefault(f=>f.Id == Guid.Parse(itemId));

            existingOrder.Items.Remove(existingProduct);
            _orderRepository.Update(existingOrder);
            _unitOfWork.Commit().Wait();

            var total = existingOrder.Items.Sum(x=>x.Price);
            var discountAmount = _orderService.CalculateDiscountAmount(existingOrder.Customer.Group.Discount, total);
            var grandTotal = total - discountAmount;

            var allProducts = _productRepository.GetAll().Result;
            var allCustomers = _customerRepository.GetAll().Result;
            CreateOrderViewModel order = new CreateOrderViewModel
            {
                OrderId = existingOrder.Id.ToString(),
                SelectedItems = existingOrder.Items.ToList(),
                TotalAmount = total,
                DiscountAmount = discountAmount,
                GrandTotalAmount = grandTotal,
                Discount = existingOrder.Customer.Group.Discount,
                Customers = GetCustomerItemList(allCustomers),
                Items = GetProductItemList(allProducts),
                SelectedCustomerId = existingOrder.Customer.Id.ToString()
            };

            return View("Create", order);
        }

        public IActionResult Edit(string orderId)
        {
            var existingOrder = _orderRepository.GetById(Guid.Parse(orderId)).Result;

            var allProducts = _productRepository.GetAll().Result;
            var allCustomers = _customerRepository.GetAll().Result;
            CreateOrderViewModel order = new CreateOrderViewModel
            {
                OrderId = existingOrder.Id.ToString(),
                SelectedItems = existingOrder.Items.ToList(),
                TotalAmount = existingOrder.Total,
                DiscountAmount = existingOrder.Discount,
                GrandTotalAmount = existingOrder.GrandTotal,
                Discount = existingOrder.Customer.Group.Discount,
                Customers = GetCustomerItemList(allCustomers),
                Items = GetProductItemList(allProducts),
                SelectedCustomerId = existingOrder.Customer.Id.ToString()
            };

            return View("Create", order);
        }

        public IActionResult Delete(string orderId)
        {
            _orderRepository.Remove(Guid.Parse(orderId));
            _unitOfWork.Commit().Wait();

            var allOrders = _orderRepository.GetAll().Result;
            return View("Index", allOrders);
        }

        public IActionResult Payment(string orderId)
        {
            var existingOrder = _orderRepository.GetById(Guid.Parse(orderId)).Result;
            var existiingPayment = _paymentRepository.GetPaymentDetails(existingOrder).Result;
            PaymentViewModel payment = new PaymentViewModel
            {
                OrderId = existingOrder.Id.ToString(),
                SelectedItems = existingOrder.Items.ToList(),
                TotalAmount = existingOrder.Total,
                DiscountAmount = existingOrder.Discount,
                GrandTotalAmount = existingOrder.GrandTotal,
                Discount = existingOrder.Customer.Group.Discount,
                Customer = existingOrder.Customer,
                PaymentStatus = (existiingPayment != null && existiingPayment.IsPaid)?  "Paid" : "Pending"
            };
            return View(payment);
        }
        public IActionResult Checkout(string orderId)
        {
            var existingOrder = _orderRepository.GetById(Guid.Parse(orderId)).Result;
            var existiingPayment = _paymentRepository.GetPaymentDetails(existingOrder).Result;
            if(existiingPayment == null)
            {
                _paymentRepository.MakePayment(existingOrder).Wait();
            }

            var allOrders = _orderRepository.GetAll().Result;
            return View("Index", allOrders);
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
