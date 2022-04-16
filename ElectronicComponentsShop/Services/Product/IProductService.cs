using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicComponentsShop.DTOs;

namespace ElectronicComponentsShop.Services.Product
{
    public interface IProductService
    {
        ProductDTO GetProduct(int id);
        double GetAverageScore(int id);
        IEnumerable<ProductDTO> GetProducts(int take = 0, int skip = 0, string sortBy = null, ProductFilterDTO filter = null);
    }
}
