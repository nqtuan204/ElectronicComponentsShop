using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicComponentsShop.DTOs;

namespace ElectronicComponentsShop.Services.Cart
{
    public interface ICartService
    {
        Task<IEnumerable<ItemDTO>> GetItems(int userId);
        Task AddItem(int userId, int productId, int quantity);
        Task RemoveAll(int userId, int productId);
        Task Clear(int userId);
        Task Update(int userId, IEnumerable<ItemDTO> items);
    }
}
