using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicComponentsShop.DTOs;
using ElectronicComponentsShop.Entities;
using ElectronicComponentsShop.Database;
using Microsoft.EntityFrameworkCore;

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

        public UserDTO GetUser(string PhoneNumberOrEmail, string Password)
        {
            var user = _db.Users.FirstOrDefault(u => (u.Email == PhoneNumberOrEmail || u.PhoneNumber == PhoneNumberOrEmail) && u.Password == Password);
            if (user == null)
                return null;
            IEnumerable<string> roles = _db.UserRoles.Include(ur => ur.Role).Where(ur => ur.UserId == user.Id).Select(ur => ur.Role.Name);
            return new UserDTO(user, roles);
        }
        public UserDTO GetUser(int id)
        {
            var user = _db.Users.FirstOrDefault(user => user.Id == id);
            if (user != null)
            {
                var roles = _db.UserRoles.Include(ur => ur.Role).Where(ur => ur.UserId == user.Id).Select(ur => ur.Role.Name);
                return new UserDTO(user, roles);
            }
            return null;
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

        public IEnumerable<int> GetFavProductIds(int userId)
        {
            return _db.Favourites.Where(f => f.UserId == userId).Select(f => f.ProductId);
        }
    }
}
