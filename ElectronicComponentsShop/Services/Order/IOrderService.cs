using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicComponentsShop.DTOs;
using ElectronicComponentsShop.Models;

namespace ElectronicComponentsShop.Services.Order
{
    public interface IOrderService
    {
        IEnumerable<PaymentTypeDTO> GetAllPaymentTypes();
        Task CreateOrder(NewOrderDTO order);
        IEnumerable<UserOrderDTO> GetUserOrders(int userId, int page, int orderStateId);
        int CountNewOrders();
        int CountCompletedOrders(DateTime from, DateTime to);

        IList<OrderVM> GetOrders(string sortBy, string keyword, int orderStateId, int page);
        int CountOrders(string keyword, int orderStateId);
        Task ChangeOrderState(int id, int orderStateId);

    }
}
