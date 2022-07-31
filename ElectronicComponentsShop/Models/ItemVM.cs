using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicComponentsShop.DTOs;
namespace ElectronicComponentsShop.Models
{
    public class ItemVM
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductThumbnailURL { get; set; }
        public string Price { get; set; }
        public int Quantity { get; set; }
        public string TotalPrice { get; set; }

        public ItemVM() { }

        public ItemVM(ItemDTO dto)
        {
            ProductId = dto.ProductId;
            ProductName = dto.ProductName;
            ProductThumbnailURL = dto.ProductThumbnailURL;
            Price = GetFormattedPrice(dto.Price, dto.Quantity);
            Quantity = dto.Quantity;
            TotalPrice = GetFormattedPrice(dto.Price, dto.Quantity);
        }

        private string GetFormattedPrice(decimal? price,int quantity)
        {

            if (price == null || price == 0)
                return "Liên hệ";
            price *= quantity;
            string priceString = ((long)price).ToString();
            int selectedNumbers = 0;
            while (priceString.Length - (selectedNumbers + 3) >= 1)
            {
                selectedNumbers += 3;
                priceString = priceString.Insert(priceString.Length - selectedNumbers, ".");
                selectedNumbers++;
            }
            return priceString;
        }

    }
}
