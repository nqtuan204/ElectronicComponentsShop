using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicComponentsShop.DTOs;

namespace ElectronicComponentsShop.Models
{
    public class ProductFilterVM
    {
        public string Categories { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string Keyword { get; set; }

        public ProductFilterVM() { }

        public ProductFilterVM(ProductFilterDTO dto)
        {
            Keyword = dto.Keyword;
            MaxPrice = dto.MaxPrice;
            MinPrice = dto.MinPrice;
            Categories = GetCategories(dto.CategoryIds);
        }

        private string GetCategories(IEnumerable<int> ids)
        {
            string result = "";
            foreach (var id in ids)
                result += id + ",";
            return result.Substring(0, result.Length - 1);
        }
    }
}
