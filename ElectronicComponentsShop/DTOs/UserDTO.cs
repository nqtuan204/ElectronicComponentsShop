using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicComponentsShop.Entities;

namespace ElectronicComponentsShop.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public IEnumerable<string> Roles { get; set; }

        public UserDTO() { }
        public UserDTO(User user, IEnumerable<string> roles)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            Roles = roles;
        }
    }
}
