using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ElectronicComponentsShop.Models
{
    public class NewProduct
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string ThumbnailURL { get; set; }
        public int CategoryId { get; set; }
    }
}
