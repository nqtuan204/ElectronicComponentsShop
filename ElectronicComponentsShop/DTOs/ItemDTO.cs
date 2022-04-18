using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicComponentsShop.Entities;

namespace ElectronicComponentsShop.DTOs
{
    public class ItemDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductURL { get; set; }
        public string ProductThumbnailURL { get; set; }
        public decimal? Price { get; set; }
        public int Quantity { get; set; }

        public ItemDTO() { }
        public ItemDTO(CartItem item)
        {
            ProductId = item.ProductId;
            ProductName = item.Product.Name;
            ProductURL = $"/product/{item.Product.Id}.{item.Product.Slug}";
            ProductThumbnailURL = item.Product.ThumbnailURL;
            Price = item.Product.Price;
            Quantity = item.Quantity;
        }

        public ItemDTO(OrderItem item)
        {
            ProductId = item.ProductId;
            ProductName = item.Product.Name;
            ProductURL = $"/product/{item.Product.Id}.{item.Product.Slug}";
            ProductThumbnailURL = item.Product.ThumbnailURL;
            Price = item.Product.Price;
            Quantity = item.Quantity;
        }
    }
}
