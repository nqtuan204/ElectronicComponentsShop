using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ElectronicComponentsShop.Models
{
    public class ResetPasswordVM
    {
        [Required(ErrorMessage = "Bạn chưa nhập email.")]
        [Remote("IsExist","User", ErrorMessage = "Email không tồn tại trong hệ thống, hãy nhập đúng email tài khoản của bạn.")]
        public string Email { get; set; }
    }
}
