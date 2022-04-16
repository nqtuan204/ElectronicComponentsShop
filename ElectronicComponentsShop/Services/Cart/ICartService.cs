using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicComponentsShop.DTOs;

namespace ElectronicComponentsShop.Services.Cart
{
    public interface ICartService
    {
        Task<CartDTO> GetCart(int userId);
        Task Add(int userId, int productId, int quantity);
    }
}
