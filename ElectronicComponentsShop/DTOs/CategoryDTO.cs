using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicComponentsShop.Entities;

namespace ElectronicComponentsShop.DTOs
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ThumbnailURL { get; set; }
        public int NumberOfProducts { get; set; }
        public CategoryDTO(Category category, int numberOfProducts)
        {
            Id = category.Id;
            Name = category.Name;
            ThumbnailURL = category.ThumbnailURL;
            NumberOfProducts = numberOfProducts;
        }
    }
}
