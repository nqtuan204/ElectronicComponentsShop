using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicComponentsShop.DTOs;

namespace ElectronicComponentsShop.Services.Cart
{
    public interface ICartService
    {
        Task<IEnumerable<CartItemDTO>> GetItems(int userId);
        Task AddItem(int userId, int productId);
        Task RemoveAll(int userId, int productId);
        Task Clear(int userId);
        Task Update(int userId, IEnumerable<CartItemDTO> items);
    }
}
