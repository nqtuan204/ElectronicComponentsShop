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
        bool IsResetPasswordTokenNullOrExpire(string resetPasswordToken);
        Task Add(NewUserDTO dto);
        Task AddRoles(int userId, IEnumerable<int> roles);
        Task AddRole(int userId, int roleId);
        UserDTO GetLoginUser(string PhoneNumberOrEmail, string Password);
        UserDTO GetUserById(int id);
        UserDTO GetUserByEmail(string Email);
        UserDTO GetUserByResetPasswordToken(string resetPasswordToken);
        Task AddToFavourites(int userId, int productId);
        int GetNumOfFavourites(int userId);
        IEnumerable<int> GetFavProductIds(int userID);
        Task RemoveFromFavourites(int userId, int productId);
        Task ChangePassword(int userId, string newPassword);
        Task<string> ResetPassword(int userId);
        Task<string> GenerateResetPasswordToken(int userId);
    }
}
