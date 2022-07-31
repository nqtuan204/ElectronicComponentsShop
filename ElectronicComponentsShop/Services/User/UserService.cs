using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicComponentsShop.DTOs;
using ElectronicComponentsShop.Entities;
using ElectronicComponentsShop.Database;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;

namespace ElectronicComponentsShop.Services.User
{
    public class UserService : IUserService
    {
        private readonly ECSDbContext _db;
        public UserService(ECSDbContext db)
        {
            _db = db;
        }
        public async Task Add(NewUserDTO dto)
        {
            var user = new Entities.User(dto);
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
            await AddRole(user.Id, 1);
        }
        public async Task AddRole(int userId, int roleId)
        {
            await _db.UserRoles.AddAsync(new UserRole(userId, roleId));
        }
        public async Task AddRoles(int userId, IEnumerable<int> roleIds)
        {
            await _db.UserRoles.AddRangeAsync(roleIds.Select(id => new UserRole(userId, id)));
        }

        public bool IsEmailExist(string Email)
        {
            return _db.Users.Any(u => u.Email == Email);
        }
        public bool IsPhoneNumberExist(string PhoneNumber)
        {
            return _db.Users.Any(u => u.PhoneNumber == PhoneNumber);
        }

        public UserDTO GetLoginUser(string PhoneNumberOrEmail, string Password)
        {
            var user = _db.Users.FirstOrDefault(u => (u.Email == PhoneNumberOrEmail || u.PhoneNumber == PhoneNumberOrEmail) && u.Password == Password);
            if (user == null)
                return null;
            IEnumerable<string> roles = _db.UserRoles.Include(ur => ur.Role).Where(ur => ur.UserId == user.Id).Select(ur => ur.Role.Name);
            return new UserDTO(user, roles);
        }
        public UserDTO GetUserById(int id)
        {
            var user = _db.Users.FirstOrDefault(user => user.Id == id);
            if (user != null)
            {
                var roles = _db.UserRoles.Include(ur => ur.Role).Where(ur => ur.UserId == user.Id).Select(ur => ur.Role.Name);
                return new UserDTO(user, roles);
            }
            return null;
        }

        public UserDTO GetUserByEmail(string Email)
        {
            var user = _db.Users.FirstOrDefault(user => user.Email == Email);
            if (user == null)
                return null;
            var roles = _db.UserRoles.Include(ur => ur.Role).Where(ur => ur.UserId == user.Id).Select(ur => ur.Role.Name);
            return new UserDTO(user, roles);
        }

        public UserDTO GetUserByResetPasswordToken(string token)
        {
            var user = _db.Users.FirstOrDefault(user => user.ResetPasswordToken == token);
            if (user == null)
                return null;
            var roles = _db.UserRoles.Include(ur => ur.Role).Where(ur => ur.UserId == user.Id).Select(ur => ur.Role.Name);
            return new UserDTO(user, roles);
        }
        public bool IsResetPasswordTokenNullOrExpire(string resetPasswordToken)
        {
            var user = _db.Users.FirstOrDefault(user => user.ResetPasswordToken == resetPasswordToken);
            if (user != null && DateTime.Now < user.ResetPasswordTokenExpireAt)
                return false;
            return true;

        }

        public async Task AddToFavourites(int userId, int productId)
        {
            await _db.Favourites.AddAsync(new Favourite(userId, productId));
            await _db.SaveChangesAsync();
        }

        public async Task RemoveFromFavourites(int userId, int productId)
        {
            var fav = _db.Favourites.FirstOrDefault(f => f.UserId == userId && f.ProductId == productId);
            if (fav != null)
            {
                _db.Favourites.Remove(fav);
                await _db.SaveChangesAsync();
            }
        }
        public int GetNumOfFavourites(int userId)
        {
            return _db.Favourites.Count(f => f.UserId == userId);
        }

        public IEnumerable<ProductDTO> GetFavProducts(int userId)
        {
            return _db.Favourites.Include(f => f.Product).Where(f => f.UserId == userId).Select(f => new ProductDTO(f.Product, 0));
        }


        public async Task ChangePassword(int userId, string newPassword)
        {
            var user = _db.Users.Find(userId);
            user.Password = newPassword;
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }

        public async Task<string> ResetPassword(int userId)
        {
            var user = _db.Users.Find(userId);
            user.Password = GenerateNewPassword();
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
            return user.Password;
        }

        private string GenerateNewPassword()
        {
            var rd = new Random();
            char[] randoms = new char[10];
            randoms = randoms.Select(e => (char)rd.Next(97, 123)).ToArray();
            return new string(randoms);
        }

        public async Task<string> GenerateResetPasswordToken(int userId)
        {
            var user = _db.Users.Find(userId);
            user.ResetPasswordTokenExpireAt = DateTime.Now.AddMinutes(5);
            using Aes crypto = Aes.Create();
            crypto.GenerateKey();
            user.ResetPasswordToken = Convert.ToBase64String(crypto.Key);
            await _db.SaveChangesAsync();
            return user.ResetPasswordToken;
        }

        public bool IsPasswordMatch(int userId, string password)
        {
            var user = _db.Users.Find(userId);
            return (password == user.Password);
        }

        public void Update(UserDTO dto)
        {
            var user = _db.Users.Find(dto.Id);
            user.PhoneNumber = dto.PhoneNumber;
            user.Email = dto.Email;
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            _db.Users.Update(user);

        }
    }
}
