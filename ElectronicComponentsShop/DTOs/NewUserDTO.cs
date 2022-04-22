using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicComponentsShop.Models;
using ElectronicComponentsShop.Entities;

namespace ElectronicComponentsShop.DTOs
{
    public class NewUserDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public NewUserDTO(NewUserVM user)
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            Password = user.Password;
        }

    }
}
