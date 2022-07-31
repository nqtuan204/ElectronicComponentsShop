using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicComponentsShop.DTOs;

namespace ElectronicComponentsShop.Services.Order
{
    public interface IOrderService
    {
        IEnumerable<PaymentTypeDTO> GetAllPaymentTypes();
        Task CreateOrder(NewOrderDTO order);
        IEnumerable<UserOrderDTO> GetUserOrders(int userId, int page, int orderStateId);
    }
}
