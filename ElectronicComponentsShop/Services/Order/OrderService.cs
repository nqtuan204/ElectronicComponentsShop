﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicComponentsShop.Database;
using ElectronicComponentsShop.Entities;
using ElectronicComponentsShop.DTOs;

namespace ElectronicComponentsShop.Services.Order
{
    public class OrderService : IOrderService
    {
        private readonly ECSDbContext _db;
        public OrderService(ECSDbContext db)
        {
            _db = db;
        }

        public Dictionary<int, string> GetAllPaymentTypes()
        {
            var paymentTypes = _db.PaymentTypes.ToDictionary(pt => pt.Id, pt => pt.Name);
            return paymentTypes;
        }

        public async Task CreateOrder(NewOrderDTO dto)
        {
            var order = new Entities.Order(dto);
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();
            await _db.OrderItems.AddRangeAsync(dto.Items.Select(item => new OrderItem(order.Id, item.Key, item.Value)));
            await _db.SaveChangesAsync();
        }
    }
}
