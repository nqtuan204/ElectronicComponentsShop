using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicComponentsShop.Entities;
using ElectronicComponentsShop.DTOs;
using ElectronicComponentsShop.Database;
using Microsoft.EntityFrameworkCore;
namespace ElectronicComponentsShop.Services.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly ECSDbContext _db;
        public CategoryService(ECSDbContext db)
        {
            _db = db;
        }
        public CategoryDTO GetCategory(int id)
        {
            var category = _db.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
                return null;
            int numOfProducts = _db.Products.Where(p => p.CategoryId == id).Count();
            return new CategoryDTO(category, numOfProducts);
        }
        public IEnumerable<CategoryDTO> GetCategories()
        {
            return _db.Categories.Include(c => c.Products).Select(c => new CategoryDTO(c, c.Products.Count()));
        }
    }
}
