using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicComponentsShop.DTOs;
using ElectronicComponentsShop.Entities;
namespace ElectronicComponentsShop.Services.Category
{
    public interface ICategoryService
    {
        CategoryDTO GetCategory(int id);
        IEnumerable<CategoryDTO> GetCategories();
    }
}
