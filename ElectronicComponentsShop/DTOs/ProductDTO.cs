using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicComponentsShop.Entities;

namespace ElectronicComponentsShop.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public string Slug { get; set; }
        public string ThumbnailURL { get; set; }
        public DateTime CreatedAt { get; set; }
        public double AverageScore { get; set; }
        public ProductDTO(Product product, double averageScore)
        {
            Id = product.Id;
            Name = product.Name;
            Price = product.Price;
            Slug = product.Slug;
            ThumbnailURL = product.ThumbnailURL;
            CreatedAt = product.CreatedAt;
            AverageScore = averageScore;
        }
    }
}
