using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicComponentsShop.DTOs;
using System.ComponentModel.DataAnnotations;
namespace ElectronicComponentsShop.Models
{
    public class CheckoutVM
    {
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
        public string Email { get; set; }
        public string FullName { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được phép bỏ trống.")]
        [RegularExpression(@"^(03[2-9]|05[689]|07[06-9]|08[1-9]|09[0-46-9])\d{7}$", ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Địa chỉ không được phép bỏ trống bất kỳ trường nào.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Địa chỉ không được phép bỏ trống bất kỳ trường nào.")]
        public int ProvinceId { get; set; }
        public string Province { get; set; }

        [Required(ErrorMessage = "Địa chỉ không được phép bỏ trống bất kỳ trường nào.")]
        public int DistrictId { get; set; }
        public string District { get; set; }

        [Required(ErrorMessage = "Địa chỉ không được phép bỏ trống bất kỳ trường nào.")]
        public int WardId { get; set; }
        public string Ward { get; set; }

        [MaxLength(500, ErrorMessage = "Ghi chú không vượt quá 500 ký tự")]
        public string Note { get; set; }
        public string Amount { get; set; }
        public IEnumerable<PaymentTypeDTO> PaymentTypes { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn hình thức thanh toán.")]
        public int PaymentTypeId { get; set; }
        public IEnumerable<ItemVM> Items { get; set; }
        public CheckoutVM() { }
        public CheckoutVM(UserDTO user, IEnumerable<ItemVM> items, IEnumerable<PaymentTypeDTO> paymentTypes, decimal amount)
        {
            Email = user.Email;
            FullName = $"{user.LastName} {user.FirstName}";
            PhoneNumber = user.PhoneNumber;
            PaymentTypes = paymentTypes;
            Items = items;
            Amount = amount == 0 ? "Liên hệ" : amount.ToString("0,0").Replace(',', '.');
        }
    }
}
