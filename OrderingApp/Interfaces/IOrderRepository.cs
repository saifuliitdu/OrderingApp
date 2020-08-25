﻿using OrderingApp.Models;
using OrderingApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderingApp.Repository
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<bool> PlaceOrder(OrderViewModel model);
        Task<bool> AddItemToOrder(Order order, Product product);
        Task<bool> RemoveItemFromOrder(Order order, Product product);
    }
}