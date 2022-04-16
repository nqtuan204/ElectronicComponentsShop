using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicComponentsShop.DTOs;
using ElectronicComponentsShop.Entities;
using ElectronicComponentsShop.Database;
using Microsoft.EntityFrameworkCore;

namespace ElectronicComponentsShop.Services.Cart
{
    public class CartService : ICartService
    {
        private readonly ECSDbContext _db;
        public CartService(ECSDbContext db)
        {
            _db = db;
        }
        public async Task<CartDTO> GetCart(int userId)
        {
            var items = await _db.CartItems.Include(item => item.Product).Where(item => item.UserId == userId).Select(item => new CartItemDTO(item)).ToListAsync();
            return new CartDTO(userId, items);
        }

        public async Task Add(int userId, int productId, int quantity)
        {
            var item = _db.CartItems.FirstOrDefault(item => item.UserId == userId && item.ProductId == productId);
            if (item == null)
            {
                _db.CartItems.Add(new CartItem(userId, productId, quantity));
                await _db.SaveChangesAsync();
            }
            else
            {
                item.Quantity += quantity;
                await _db.SaveChangesAsync();
            }

        }
    }
}
