using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicComponentsShop.DTOs;

namespace ElectronicComponentsShop.Models
{
    public class CategoryVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public string ThumbnailURL { get; set; }
        public int NumberOfProducts { get; set; }
        public CategoryVM(CategoryDTO dto)
        {
            Id = dto.Id;
            Name = dto.Name;
            URL = $"/product/list?categories={dto.Id}";
            ThumbnailURL = dto.ThumbnailURL;
            NumberOfProducts = dto.NumberOfProducts;
        }
    }
}
