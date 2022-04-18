using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicComponentsShop.DTOs;

namespace ElectronicComponentsShop.Services.User
{
    public interface IUserService
    {
        bool IsEmailExist(string Email);
        bool IsPhoneNumberExist(string PhoneNumber);
        Task Add(NewUserDTO dto);
        Task AddRoles(int userId, IEnumerable<int> roles);
        Task AddRole(int userId, int roleId);
        UserDTO GetUser(string PhoneNumberOrEmail, string Password);
        UserDTO GetUser(int id);
        Task AddToFavourites(int userId, int productId);
        int GetNumOfFavourites(int userId);
        IEnumerable<int> GetFavProductIds(int userID);
        Task RemoveFromFavourites(int userId, int productId);
    }
}
