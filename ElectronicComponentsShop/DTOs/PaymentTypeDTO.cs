using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicComponentsShop.Entities;

namespace ElectronicComponentsShop.DTOs
{
    public class PaymentTypeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public string Description { get; set; }
        public PaymentTypeDTO(PaymentType paymentType)
        {
            Id = paymentType.Id;
            Name = paymentType.Name;
        }
    }
}
