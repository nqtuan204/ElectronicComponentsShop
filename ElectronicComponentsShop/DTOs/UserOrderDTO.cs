﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicComponentsShop.Entities;

namespace ElectronicComponentsShop.DTOs
{
    public class UserOrderDTO
    {
        public int Id { get; set; }
        public int OrderStateId { get; set; }
        public string OrderStateName { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> ModifiedAt { get; set; }
        public IEnumerable<ItemDTO> Items { get; set; }
        public UserOrderDTO(Order order)
        {
            Id = order.Id;
            OrderStateId = order.OrderStateId;
            OrderStateName = order.OrderState.Name;
            CreatedAt = order.CreatedAt;
            ModifiedAt = order.ModifiedAt;
            Items = order.Items.Select(item => new ItemDTO(item));
        }
    }
}
