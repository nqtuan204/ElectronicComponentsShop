using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicComponentsShop.Entities;

namespace ElectronicComponentsShop.Models
{
    public class ProductDataVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CategoryId { get; set; }
        public string Category { get; set; }
        public decimal? Price { get; set; }
        public string ThumbnailURL { get; set; }

        public ProductDataVM() { }

        public ProductDataVM(Product product)
        {
            Id = product.Id;
            Name = product.Name;
            CreatedAt = product.CreatedAt;
            CategoryId = product.CategoryId;
            Category = product.Category.Name;
            Price = product.Price;
            ThumbnailURL = product.ThumbnailURL;
        }
    }
}
