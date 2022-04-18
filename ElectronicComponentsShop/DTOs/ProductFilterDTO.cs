using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicComponentsShop.Models;

namespace ElectronicComponentsShop.DTOs
{
    public class ProductFilterDTO
    {
        public List<int> CategoryIds { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string Keyword { get; set; }

        public ProductFilterDTO(ProductFilterVM vm)
        {
            if (!String.IsNullOrEmpty(vm.Categories))
                CategoryIds = vm.Categories.Split(",").Select(i => int.Parse(i)).ToList();
            MinPrice = vm.MinPrice;
            MaxPrice = vm.MaxPrice;
            Keyword = vm.Keyword;
        }
    }
}
