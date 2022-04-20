using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicComponentsShop.Entities;

namespace ElectronicComponentsShop.DTOs
{
    public class ProductDetailsDTO
    {
        public int Id { get; set; }
        public string Slug { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public ProductDetailsDTO(Product product)
        {
            Id = product.Id;
            Slug = product.Slug;
            Name = product.Name;
            Price = product.Price;
            Description = product.Description;
            CategoryId = product.CategoryId;
            CategoryName = product.Category.Name;
        }
    }
}
