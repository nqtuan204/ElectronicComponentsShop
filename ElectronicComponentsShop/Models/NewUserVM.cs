using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicComponentsShop.Models
{
    public class NewUserVM
    {
        [Required(ErrorMessage = "Số điện thoại không được phép bỏ trống.")]
        [RegularExpression(@"^(03[2-9]|05[689]|07[06-9]|08[1-9]|09[0-46-9])\d{7}$", ErrorMessage = "Số điện thoại không hợp lệ.")]
        [Remote("IsPhoneNumberExist", "User", HttpMethod = "Post", ErrorMessage = "Số điện thoại này đã được sử dụng để đăng ký.")]
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }

        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        [Remote("IsEmailExist", "User", HttpMethod = "Post", ErrorMessage = "Email này đã được sử dụng để đăng ký.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được phép bỏ trống.")]
        [RegularExpression(@"[\w]{6,}", ErrorMessage = "Mật khẩu phải có tối thiểu 6 ký tự")]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Các mật khẩu đã nhập không khớp. Hãy thử lại.")]
        [Display(Name = "Xác nhận mật khẩu")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Mật khẩu 2 không được phép bỏ trống.")]
        [RegularExpression(@"[\w]{6,}", ErrorMessage = "Mật khẩu 2 phải có tối thiểu 6 ký tự")]
        [Display(Name = "Mật khẩu")]
        public string Password2 { get; set; }

        [Compare("Password2", ErrorMessage = "Các mật khẩu đã nhập không khớp. Hãy thử lại.")]
        [Display(Name = "Xác nhận mật khẩu")]
        public string ConfirmPassword2 { get; set; }

        [Required(ErrorMessage = "Hãy nhập tên của bạn.")]
        [RegularExpression(@"[A-Za-zưùúủũụừứửữựêèéẻẽẹềếểễệôơòóỏõọồốỏõọờớởỡợâăàáảãạầấẩẫậằắẳẵặìíỉĩị\s]{2,}", ErrorMessage = "Vui lòng nhập đúng tên của bạn.")]
        [Display(Name = "Tên")]
        public string FirstName { get; set; }

        [RegularExpression(@"[A-Za-zưùúủũụừứửữựêèéẻẽẹềếểễệôơòóỏõọồốỏõọờớởỡợâăàáảãạầấẩẫậằắẳẵặìíỉĩị]{2,}", ErrorMessage = "Vui lòng nhập đúng họ của bạn.")]
        [Required(ErrorMessage = "Hãy nhập họ của bạn.")]
        [Display(Name = "Họ")]
        public string LastName { get; set; }
    }
}
