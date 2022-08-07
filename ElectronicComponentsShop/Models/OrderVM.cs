using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ElectronicComponentsShop.Entities;

namespace ElectronicComponentsShop.Models
{
    public class OrderVM
    {
        [Display(Name = "Mã đơn hàng")]
        public int Id { get; set; }
        [Display(Name = "Ngày tạo")]
        public DateTime CreatedAt { get; set; }
        [Display(Name = "Cập nhật lần cuối")]
        public DateTime? ModifiedAt { get; set; }
        [Display(Name = "Mã khách hàng")]
        public int UserId { get; set; }
        [Display(Name = "Trạng thái")]
        public string OrderState { get; set; }
        [Display(Name = "Hình thức thanh toán")]
        public string PaymentType { get; set; }
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        public OrderVM(int id, DateTime createdAt, DateTime modifiedAt, int userId, string orderState, string paymentType, string address)
        {
            Id = id;
            CreatedAt = createdAt;
            ModifiedAt = modifiedAt;
            UserId = userId;
            OrderState = orderState;
            PaymentType = paymentType;
            Address = address;
        }

        public OrderVM(Order order)
        {
            Id = order.Id;
            CreatedAt = order.CreatedAt;
            ModifiedAt = order.ModifiedAt;
            UserId = order.UserId;
            OrderState = order.OrderState.Name;
            PaymentType = order.PaymentType.Name;
            Address = $"{order.Address} - {order.Ward.Name} - {order.Ward.District.Name} - {order.Ward.District.Province.Name}";
        }
    }
}
