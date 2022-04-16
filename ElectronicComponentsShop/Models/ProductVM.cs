using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicComponentsShop.DTOs;

namespace ElectronicComponentsShop.Models
{
    public class ProductVM
    {
        //ProductVM: Id,Name,FormattedPrice,URL,ThumbnailURL,AverageScore
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string FormattedPrice { get; set; }
        public string URL { get; set; }
        public string ThumbnailURL { get; set; }
        public double AverageScore { get; set; }
        public bool IsNew { get; set; }
        public ProductVM(ProductDTO dto)
        {
            Id = dto.Id;
            Name = dto.Name;
            ShortName = dto.Name.Length>24?dto.Name.Substring(0, 20)+"...":dto.Name;
            FormattedPrice = GetFormattedPrice(dto.Price);
            URL = $"/product/{dto.Id}.{dto.Slug}";
            ThumbnailURL = dto.ThumbnailURL;
            AverageScore = dto.AverageScore;
            IsNew = (DateTime.Now - dto.CreatedAt).TotalDays <= 30 ? true : false;
        }

        private string GetFormattedPrice(decimal? price)
        {
            if (price == null || price == 0)
                return "Liên hệ";
            string priceString = ((long)price).ToString();
            int selectedNumbers = 0;
            while (priceString.Length - (selectedNumbers + 3) >= 1)
            {
                selectedNumbers += 3;
                priceString = priceString.Insert(priceString.Length - selectedNumbers, ".");
                selectedNumbers++;
            }
            return priceString + "đ";
        }
    }
}
