using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicComponentsShop.DTOs
{
    public class CartDTO
    {
        public int UserId { get; set; }
        public IEnumerable<CartItemDTO> Items { get; set; }
        public CartDTO(int userId, IEnumerable<CartItemDTO> items)
        {
            UserId = userId;
            Items = items;
        }
    }
}
