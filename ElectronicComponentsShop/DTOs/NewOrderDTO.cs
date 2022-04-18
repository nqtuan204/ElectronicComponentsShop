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
        public string Province { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public int PaymentTypeId { get; set; }
        public string Note { get; set; }
        public Dictionary<int, int> Items { get; set; }

        public NewOrderDTO(int userId, CheckoutVM checkout)
        {
            UserId = userId;
            Address = checkout.Address;
            Province = checkout.Province;
            District = checkout.District;
            Ward = checkout.Ward;
            PaymentTypeId = checkout.payment;
            Note = checkout.Note;
            Items = checkout.Items.ToDictionary(i => i.ProductId, i => i.Quantity);
        }
    }
}
