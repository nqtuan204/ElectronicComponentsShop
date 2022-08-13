using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicComponentsShop.Database;
using ElectronicComponentsShop.Entities;
using ElectronicComponentsShop.DTOs;
using Microsoft.EntityFrameworkCore;
using ElectronicComponentsShop.Models;

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
            var userOrders = from o in _db.Orders where o.UserId == userId select o;
            if (orderStateId > 0)
                userOrders = userOrders.Where(o => o.OrderStateId == orderStateId);
            return userOrders.AsSplitQuery().Include(o => o.OrderState).Include(o => o.Items).ThenInclude(i => i.Product).OrderByDescending(o => o.CreatedAt).Skip(0).Take(5 * page).Select(o => new UserOrderDTO(o));
        }

        public int CountNewOrders()
        {
            return _db.Orders.Where(o => o.OrderStateId == 1).Count();
        }

        public int CountCompletedOrders(DateTime from, DateTime to)
        {
            return _db.Orders.Where(o => o.OrderStateId == 4 && o.ModifiedAt >= from && o.ModifiedAt <= to).Count();
        }

        public int CountOrders(string keyword, int orderStateId)
        {
            var orders = from o in _db.Orders select o;
            if (orderStateId > 0 && orderStateId <= 4)
                orders = orders.Where(o => o.OrderStateId == orderStateId);
            orders = orders.AsSplitQuery().Include(o => o.PaymentType).Include(o => o.OrderState).Include(o => o.Ward).ThenInclude(w => w.District).ThenInclude(d => d.Province);
            if (!String.IsNullOrEmpty(keyword))
                orders = orders.Where(o => o.Id.ToString() == keyword || o.CreatedAt.ToString().Contains(keyword) || (o.ModifiedAt.HasValue && o.ModifiedAt.ToString().Contains(keyword)) || o.UserId.ToString() == keyword || o.PaymentType.Name.Contains(keyword) || o.OrderState.Name.Contains(keyword) || o.Address.Contains(keyword) || o.Ward.Name.Contains(keyword) || o.District.Name.Contains(keyword) || o.Province.Name.Contains(keyword));
            return orders.Count();
        }

        public IList<OrderVM> GetOrders(string sortBy, string keyword, int orderStateId, int page)
        {
            var orders = (from o in _db.Orders select o);

            if (!String.IsNullOrEmpty(sortBy))
            {
                if (sortBy.Contains("asc"))
                {
                    orders = orders.OrderBy(o => o.ModifiedAt);
                    if (sortBy.Contains("orderId"))
                        orders = orders.OrderBy(o => o.Id);
                    if (sortBy.Contains("createdAt"))
                        orders = orders.OrderBy(o => o.CreatedAt);
                    if (sortBy.Contains("userName"))
                        orders = orders.OrderBy(o => o.User.LastName);
                    if (sortBy.Contains("userPhoneNumber"))
                        orders = orders.OrderBy(o => o.User.PhoneNumber);
                    if (sortBy.Contains("orderStateId"))
                        orders = orders.OrderBy(o => o.OrderState.Name);
                    if (sortBy.Contains("paymentTypeId"))
                        orders = orders.OrderBy(o => o.PaymentType.Name);
                }
                else
                {
                    orders = orders.OrderByDescending(o => o.ModifiedAt);
                    if (sortBy.Contains("orderId"))
                        orders = orders.OrderByDescending(o => o.Id);
                    if (sortBy.Contains("createdAt"))
                        orders = orders.OrderByDescending(o => o.CreatedAt);
                    if (sortBy.Contains("userName"))
                        orders = orders.OrderByDescending(o => o.User.LastName);
                    if (sortBy.Contains("userPhoneNumber"))
                        orders = orders.OrderByDescending(o => o.User.PhoneNumber);
                    if (sortBy.Contains("orderStateId"))
                        orders = orders.OrderByDescending(o => o.OrderState.Name);
                }

            }
            if (orderStateId > 0 && orderStateId <= 4)
                orders = orders.Where(o => o.OrderStateId == orderStateId);
            orders = orders.AsSplitQuery().Include(o => o.PaymentType).Include(o => o.OrderState).Include(o => o.User).Include(o => o.Ward).ThenInclude(w => w.District).ThenInclude(d => d.Province);
            if (!String.IsNullOrEmpty(keyword))
                orders = orders.Where(o => o.Id.ToString() == keyword || o.CreatedAt.ToString().Contains(keyword) || (o.ModifiedAt.HasValue && o.ModifiedAt.ToString().Contains(keyword)) || $"{o.User.LastName} {o.User.FirstName}".Contains(keyword) || o.PaymentType.Name.Contains(keyword) || o.OrderState.Name.Contains(keyword) || o.Address.Contains(keyword) || o.Ward.Name.Contains(keyword) || o.District.Name.Contains(keyword) || o.Province.Name.Contains(keyword) || o.User.PhoneNumber.Contains(keyword));

            return orders.Skip((page - 1) * 30).Take(30).Select(o => new OrderVM(o)).ToList();
        }

        public async Task ChangeOrderState(int id, int orderStateId)
        {
            var order = _db.Orders.FirstOrDefault(o => o.Id == id);
            order.OrderStateId = orderStateId;
            order.ModifiedAt = DateTime.Now;
            await _db.SaveChangesAsync();
        }
    }
}
