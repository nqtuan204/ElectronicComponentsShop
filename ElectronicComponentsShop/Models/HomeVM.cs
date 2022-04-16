using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicComponentsShop.Models
{
    public class HomeVM
    {
        public IEnumerable<CategoryVM> Categories { get; set; }
        public IEnumerable<ProductCarouselVM> ProductCarousels { get; set; }
        public HomeVM(IEnumerable<CategoryVM> categories, IEnumerable<ProductCarouselVM> productCarousels)
        {
            Categories = categories;
            ProductCarousels = productCarousels;
        }
    }
}
