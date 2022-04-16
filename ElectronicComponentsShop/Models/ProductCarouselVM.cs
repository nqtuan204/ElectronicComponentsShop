using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicComponentsShop.Models
{
    public class ProductCarouselVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public IEnumerable<ProductVM> Products { get; set; }
        public ProductCarouselVM(string title, IEnumerable<ProductVM> products, int id)
        {
            Title = title;
            Products = products;
            Id = id;
        }
    }
}
