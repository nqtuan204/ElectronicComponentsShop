using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicComponentsShop.DTOs
{
    public class CartDTO
    {
        public IEnumerable<CartItemDTO> Items { get; set; }
        public CartDTO()
        {

        }
        public CartDTO(IEnumerable<CartItemDTO> items)
        {
            Items = items;
        }
    }
}
