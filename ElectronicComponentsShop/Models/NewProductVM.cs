using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ElectronicComponentsShop.Entities;

namespace ElectronicComponentsShop.Models
{
    public class NewProductVM
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string ThumbnailURL { get; set; }
        public int CategoryId { get; set; }

        public NewProductVM()
        {

        }

        public NewProductVM(Product p)
        {
            Name = p.Name;
            Price = p.Price;
            Description = p.Description;
            ThumbnailURL = p.ThumbnailURL;
            CategoryId = p.CategoryId;
        }
    }
}
