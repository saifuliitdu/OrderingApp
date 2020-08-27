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
            var allOrders = _orderRepository.GetAllOrders().Result;
            
            return View(allOrders);
        }
        public IActionResult Create()
        {
            _logger.LogInformation("The create view is called.");
            var createOrderViewModel = GetCreateOrderViewMode(null);
            return View(createOrderViewModel);
        }

        [HttpPost]
        public IActionResult AddItemToOrder(CreateOrderViewModel order)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Order orderToBeSaved;
                    if (string.IsNullOrEmpty(order.OrderId))
                        orderToBeSaved = new Order();
                    else
                        orderToBeSaved = _orderRepository.GetById(Guid.Parse(order.OrderId)).Result;

                    var existingProduct = _productRepository.GetById(Guid.Parse(order.SelectedItemId)).Result;
                    var existingCustomer = _customerRepository.GetById(Guid.Parse(order.SelectedCustomerId)).Result;

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


                    var createOrderViewModel = GetCreateOrderViewMode(orderToBeSaved);
                    _logger.LogInformation("Add item to order is successful.");
                    return View("Create", createOrderViewModel);
                }
                _logger.LogWarning("Add item to order method found model state is invalid.");
                return View();
            }
            catch (Exception e)
            {
                _logger.LogWarning("Add item to order is failed. \nException: " + e.Message);
                return View();
            }
        }


        public IActionResult RemoveItemFromOrder(string orderId, string itemId)
        {
            try
            {
                var existingOrder = _orderRepository.GetById(Guid.Parse(orderId)).Result;
                var productToBeDeleted = existingOrder.Items.FirstOrDefault(f => f.Id == Guid.Parse(itemId));

                existingOrder.Items.Remove(productToBeDeleted);
                _orderRepository.Update(existingOrder);
                _unitOfWork.Commit().Wait();

                var total = existingOrder.Items.Sum(x => x.Price);
                var discountAmount = _orderService.CalculateDiscountAmount(existingOrder.Customer.Group.Discount, total);
                var grandTotal = total - discountAmount;

                existingOrder.Total = total;
                existingOrder.Discount = discountAmount;
                existingOrder.GrandTotal = grandTotal;
                var createOrderViewModel = GetCreateOrderViewMode(existingOrder);
                _logger.LogInformation("Remove item from order is successful.");
                return View("Create", createOrderViewModel);
            }
            catch (Exception e)
            {
                _logger.LogWarning("Remove item from order is failed. \nException: " + e.Message);
                return View();
            }
        }

        public IActionResult Edit(string orderId)
        {
            try
            {
                var existingOrder = _orderRepository.GetById(Guid.Parse(orderId)).Result;
                var payment = _paymentRepository.GetAll().Result.FirstOrDefault(f => f.Order.Id == existingOrder.Id);
                existingOrder.Payment = payment;

                var createOrderViewModel = GetCreateOrderViewMode(existingOrder);
                _logger.LogInformation("Edit is clled successfully.");
                return View("Create", createOrderViewModel);
            }
            catch (Exception e)
            {
                _logger.LogWarning("Edit is failed. \nException: " + e.Message);
                return View();
            }
        }

        public IActionResult Delete(string orderId)
        {
            try
            {
                _orderRepository.Remove(Guid.Parse(orderId));
                _unitOfWork.Commit().Wait();


                var allOrders = _orderRepository.GetAllOrders().Result;
                _logger.LogInformation("Delete is successful.");
                return View("Index", allOrders);
            }
            catch (Exception e)
            {
                _logger.LogWarning("Delete is failed. \nException: " + e.Message);
                return View();
            }
        }

        public IActionResult Payment(string orderId)
        {
            try
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
                    PaymentStatus = (existiingPayment != null && existiingPayment.IsPaid) ? "Paid" : "Pending"
                };
                _logger.LogInformation("Payment view is called successfully.");
                return View(payment);
            }
            catch (Exception e)
            {
                _logger.LogWarning("Payment view can't route. \nException: " + e.Message);
                return View();
            }
        }
        public IActionResult Checkout(string orderId)
        {
            try
            {
                var existingOrder = _orderRepository.GetById(Guid.Parse(orderId)).Result;
                var existiingPayment = _paymentRepository.GetPaymentDetails(existingOrder).Result;
                if (existiingPayment == null)
                {
                    _paymentRepository.MakePayment(existingOrder).Wait();
                }

                var allOrders = _orderRepository.GetAllOrders().Result;
                _logger.LogInformation("Payment confirmation is successful.");
                return View("Index", allOrders);
            }
            catch (Exception e)
            {
                _logger.LogWarning("Payment confirm is failed. \nException: " + e.Message);
                return View();
            }
        }

        private CreateOrderViewModel GetCreateOrderViewMode(Order order)
        {
            try
            {
                var allProducts = _productRepository.GetAll().Result;
                var allCustomers = _customerRepository.GetAll().Result;
                CreateOrderViewModel createOrderViewModel;
                if (order == null)
                {
                    createOrderViewModel = new CreateOrderViewModel
                    {
                        Customers = GetCustomerItemList(allCustomers),
                        Items = GetProductItemList(allProducts),
                        SelectedItems = new List<Product>()
                    };
                }
                else
                {
                    createOrderViewModel = new CreateOrderViewModel
                    {
                        Customers = GetCustomerItemList(allCustomers),
                        Items = GetProductItemList(allProducts),
                        OrderId = order.Id.ToString(),
                        SelectedItems = order.Items.ToList(),
                        TotalAmount = order.Total,
                        DiscountAmount = order.Discount,
                        GrandTotalAmount = order.GrandTotal,
                        Discount = order.Customer != null ? order.Customer.Group.Discount : 0,
                        SelectedCustomerId = order.Customer != null ? order.Customer.Id.ToString() : "",
                        IsPaid = order.Payment != null? order.Payment.IsPaid : false
                    };
                }
                _logger.LogWarning("Create order view model successfully.");
                return createOrderViewModel;
            }
            catch (Exception e)
            {
                _logger.LogWarning("Create order view model failed. \nException: " + e.Message);
                return new CreateOrderViewModel();
            }

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
