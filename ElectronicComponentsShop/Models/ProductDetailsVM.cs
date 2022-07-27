using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicComponentsShop.DTOs;

namespace ElectronicComponentsShop.Models
{
    public class ProductDetailsVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public double AverageScore { get; set; }
        public IEnumerable<string> ImageURLs { get; set; }
        public int NumOfReviews { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public ProductDetailsVM(ProductDetailsDTO dto, IEnumerable<string> imageURLs, double averageScore, int numOfReviews)
        {
            Id = dto.Id;
            Name = dto.Name;
            Description = dto.Description;
            ImageURLs = imageURLs;
            AverageScore = averageScore;
            NumOfReviews = numOfReviews;
            CategoryId = dto.CategoryId;
            CategoryName = dto.CategoryName;
            Price = GetFormattedPrice(dto.Price);
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
