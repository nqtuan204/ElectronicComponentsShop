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
        public async Task<IEnumerable<ItemDTO>> GetItems(int userId)
        {
            var items = await _db.CartItems.Include(item => item.Product).Where(item => item.UserId == userId).Select(item => new ItemDTO(item)).ToListAsync();
            return items;
        }

        public async Task AddItem(int userId, int productId)
        {
            var item = _db.CartItems.FirstOrDefault(item => item.UserId == userId && item.ProductId == productId);
            if (item == null)
            {
                _db.CartItems.Add(new CartItem(userId, productId, 1));
                await _db.SaveChangesAsync();
            }
            else
            {
                item.Quantity++;
                await _db.SaveChangesAsync();
            }
        }


        public async Task RemoveItem(int userId, int productId)
        {
            var item = await _db.CartItems.FirstAsync(item => item.UserId == userId && item.ProductId == productId);
            if (item.Quantity > 1)
                item.Quantity--;
            else
            {
                _db.CartItems.Remove(item);
                await _db.SaveChangesAsync();
            }
        }

        public async Task RemoveAll(int userId, int productId)
        {
            var item = await _db.CartItems.FirstAsync(item => item.UserId == userId && item.ProductId == productId);
            _db.CartItems.Remove(item);
            await _db.SaveChangesAsync();
        }

        public async Task Clear(int userId)
        {
            _db.CartItems.RemoveRange(_db.CartItems.Where(item => item.UserId == userId));
            await _db.SaveChangesAsync();
        }

        public async Task Update(int userId, IEnumerable<ItemDTO> items)
        {
            var productIds = items.Select(item => item.ProductId).ToList();
            var removedItems = _db.CartItems.Where(item => item.UserId == userId && !productIds.Contains(item.ProductId));
            _db.CartItems.UpdateRange(items.Select(item => new CartItem(userId, item.ProductId, item.Quantity)));
            _db.CartItems.RemoveRange(removedItems);
            await _db.SaveChangesAsync();
        }
    }
}
