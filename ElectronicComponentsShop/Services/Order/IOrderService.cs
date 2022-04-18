using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicComponentsShop.DTOs;

namespace ElectronicComponentsShop.Services.Order
{
    public interface IOrderService
    {
        Dictionary<int, string> GetAllPaymentTypes();
        Task CreateOrder(NewOrderDTO order);
    }
}
