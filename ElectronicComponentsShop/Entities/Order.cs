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
        public string Note { get; set; }
        public string Address { get; set; }
        public int ProvinceId { get; set; }
        public Province Province { get; set; }
        public int DistrictId { get; set; }
        public District District { get; set; }
        public int WardId { get; set; }
        public Ward Ward { get; set; }
        public int OrderStateId { get; set; }
        public OrderState OrderState { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public IEnumerable<OrderItem> Items { get; set; }

        public int PaymentTypeId { get; set; }
        public PaymentType PaymentType { get; set; }

        public Order() { }

        public Order(NewOrderDTO dto)
        {
            CreatedAt = DateTime.Now;
            Address = dto.Address;
            DistrictId = dto.DistrictId;
            ProvinceId = dto.ProvinceId;
            WardId = dto.WardId;
            Note = dto.Note;
            UserId = dto.UserId;
            PaymentTypeId = dto.PaymentTypeId;
        }
    }
}
