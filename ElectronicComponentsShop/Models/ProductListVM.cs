using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace ElectronicComponentsShop.Models
{
    public class ProductListVM
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public ProductFilterVM Filter { get; set; }
        public string SortBy { get; set; }
        public IEnumerable<CategoryVM> Categories { get; set; }

        public ProductListVM(int page, int pageSize, ProductFilterVM filter, string sortBy, IEnumerable<CategoryVM> categories)
        {
            Page = page;
            PageSize = pageSize;
            Filter = filter;
            SortBy = sortBy;
            Categories = categories;
        }
    }
}
