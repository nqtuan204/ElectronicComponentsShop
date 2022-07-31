using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicComponentsShop.Database;
using ElectronicComponentsShop.Entities;
using ElectronicComponentsShop.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ElectronicComponentsShop.Services.Order
{
    public class OrderService : IOrderService
    {
        private readonly ECSDbContext _db;
        public OrderService(ECSDbContext db)
        {
            _db = db;
        }

        public IEnumerable<PaymentTypeDTO> GetAllPaymentTypes()
        {
            var paymentTypes = _db.PaymentTypes.Select(p => new PaymentTypeDTO(p));
            return paymentTypes;
        }

        public async Task CreateOrder(NewOrderDTO dto)
        {
            var order = new Entities.Order(dto);
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();
            await _db.OrderItems.AddRangeAsync(dto.Items.Select(item => new OrderItem(order.Id, item.ProductId, item.Quantity, item.Price)));
            await _db.SaveChangesAsync();
        }

        public IEnumerable<UserOrderDTO> GetUserOrders(int userId, int page, int orderStateId)
        {
            var userOrders = from o in _db.Orders select o;
            if (orderStateId > 0)
                userOrders = userOrders.Where(o => o.OrderStateId == orderStateId);
            return userOrders.AsSplitQuery().Include(o => o.OrderState).Include(o => o.Items).ThenInclude(i => i.Product).OrderByDescending(o => o.CreatedAt).Skip(0).Take(5 * page).Select(o => new UserOrderDTO(o));
        }
    }
}
