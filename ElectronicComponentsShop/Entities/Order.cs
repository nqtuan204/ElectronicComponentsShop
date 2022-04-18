using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicComponentsShop.DTOs;

namespace ElectronicComponentsShop.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> ModifiedAt { get; set; }
        public string Address { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public string Note { get; set; }
        public int OrderStateId { get; set; }
        public OrderState OrderState { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public IEnumerable<OrderItem> Items { get; set; }
        public Payment Payment { get; set; }

        public int PaymentTypeId { get; set; }
        public PaymentType PaymentType { get; set; }

        public Order() { }

        public Order(NewOrderDTO dto)
        {
            CreatedAt = DateTime.Now;
            Address = dto.Address;
            District = dto.District;
            Province = dto.Province;
            Ward = dto.Ward;
            Note = dto.Note;
            OrderStateId = 1;
            UserId = dto.UserId;
            PaymentTypeId = dto.PaymentTypeId;
        }
    }
}
