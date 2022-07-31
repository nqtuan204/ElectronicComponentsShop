using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicComponentsShop.Models;

namespace ElectronicComponentsShop.DTOs
{
    public class NewOrderDTO
    {
        public int UserId { get; set; }
        public string Address { get; set; }
        public int ProvinceId { get; set; }
        public int DistrictId { get; set; }
        public int WardId { get; set; }
        public int PaymentTypeId { get; set; }
        public string Note { get; set; }
        public IEnumerable<ItemDTO> Items { get; set; }

        public NewOrderDTO(int userId, CheckoutVM checkout)
        {
            UserId = userId;
            Address = checkout.Address;
            ProvinceId = checkout.ProvinceId;
            DistrictId = checkout.DistrictId;
            WardId = checkout.WardId;
            PaymentTypeId = checkout.PaymentTypeId;
            Note = checkout.Note;
            Items = checkout.Items.Select(i => new ItemDTO(i));
        }
    }
}
