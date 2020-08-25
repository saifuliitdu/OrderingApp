using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrderingApp.Interfaces;
using OrderingApp.Models;
using OrderingApp.Repository;

namespace OrderingApp.Controllers
{
    public class OrderController : Controller
    {
        IOrderRepository _orderRepository;
        IUnitOfWork _unitOfWork;
        public OrderController(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var allProducts = _orderRepository.GetAll().Result;

            return View(allProducts);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Create([Bind] Order order)
        {
            if (ModelState.IsValid)
            {
                _orderRepository.Add(order);
                _unitOfWork.Commit();
                return RedirectToAction("Index");
            }
            return View(order);
        }
    

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
