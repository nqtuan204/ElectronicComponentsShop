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
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string UserName { get; set; }
        public string UserPhoneNumber { get; set; }
        public string OrderState { get; set; }
        public string PaymentType { get; set; }
        public string Address { get; set; }
        public OrderVM(int id, DateTime createdAt, DateTime modifiedAt, string userName, string userPhoneNumber, string orderState, string paymentType, string address)
        {
            Id = id;
            CreatedAt = createdAt;
            ModifiedAt = modifiedAt;
            UserName = userName;
            OrderState = orderState;
            PaymentType = paymentType;
            Address = address;
            UserPhoneNumber = userPhoneNumber;
        }

        public OrderVM(Order order)
        {
            Id = order.Id;
            CreatedAt = order.CreatedAt;
            ModifiedAt = order.ModifiedAt;
            UserName = $"{order.User.LastName} {order.User.FirstName}";
            OrderState = order.OrderState.Name;
            PaymentType = order.PaymentType.Name;
            Address = $"{order.Address} - {order.Ward.Name} - {order.Ward.District.Name} - {order.Ward.District.Province.Name}";
            UserPhoneNumber = order.User.PhoneNumber;
        }
    }
}
