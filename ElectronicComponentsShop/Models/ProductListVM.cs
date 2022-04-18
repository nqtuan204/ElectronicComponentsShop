using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace ElectronicComponentsShop.Models
{
    public class ProductListVM
    {
        public PaginatorVM Paginator { get; set; }
        public ProductFilterVM Filter { get; set; }
        public string SortBy { get; set; }
        public IEnumerable<CategoryVM> Categories { get; set; }
        public IEnumerable<ProductVM> Products { get; set; }

        public ProductListVM(PaginatorVM paginator, ProductFilterVM filter, string sortBy, IEnumerable<CategoryVM> categories, IEnumerable<ProductVM> products)
        {
            Paginator = paginator;
            Filter = filter;
            SortBy = sortBy;
            Categories = categories;
            Products = products;
        }
    }
}
