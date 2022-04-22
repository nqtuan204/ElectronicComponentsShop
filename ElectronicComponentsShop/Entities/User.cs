using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicComponentsShop.DTOs;

namespace ElectronicComponentsShop.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ResetPasswordToken { get; set; }
        public DateTime ResetPasswordTokenExpireAt { get; set; }
        public Nullable<DateTime> ModifiedAt { get; set; }
        public IEnumerable<UserRole> UserRoles { get; set; }
        public IEnumerable<Order> Orders { get; set; }
        public IEnumerable<CartItem> CartItems { get; set; }
        public IEnumerable<Favourite> Favourites { get; set; }
        public IEnumerable<Review> Reviews { get; set; }
        public User() { }

        public User(NewUserDTO dto)
        {
            PhoneNumber = dto.PhoneNumber;
            Email = dto.Email;
            Password = dto.Password;
            FirstName = dto.FirstName;
            LastName = dto.LastName;
            CreatedAt = DateTime.Now;

        }
    }
}
