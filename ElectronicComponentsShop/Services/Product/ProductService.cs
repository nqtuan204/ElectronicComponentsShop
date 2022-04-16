using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ElectronicComponentsShop.Entities;
using ElectronicComponentsShop.DTOs;
using ElectronicComponentsShop.Database;
using Microsoft.EntityFrameworkCore;

namespace ElectronicComponentsShop.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly ECSDbContext _db;
        public ProductService(ECSDbContext db)
        {
            _db = db;
        }
        public ProductDTO GetProduct(int id)
        {
            var product = _db.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return null;
            double averageScore = GetAverageScore(id);
            return new ProductDTO(product, averageScore);
        }
        public double GetAverageScore(int id)
        {
            return _db.Reviews.Where(r => r.ProductId == id).Average(r => r.Score);
        }
        private Expression<Func<Entities.Product, dynamic>> SortExpression(string sortBy)
        {
            if (sortBy.StartsWith("date"))
                return p => p.CreatedAt;
            if (sortBy.StartsWith("views"))
                return p => p.Views;
            if (sortBy.StartsWith("name"))
                return p => p.Name;
            if (sortBy.StartsWith("score"))
                return p => GetAverageScore(p.Id);
            return null;
        }

        private IEnumerable<Expression<Func<Entities.Product, bool>>> FilterExpressions(ProductFilterDTO filter)
        {
            var expressions = new List<Expression<Func<Entities.Product, bool>>>();
            if (filter == null)
                return expressions;
            if (filter.CategoryIds != null && filter.CategoryIds.Count > 0)
                foreach (var id in filter.CategoryIds)
                    expressions.Add(p => p.CategoryId == id);
            if (filter.MinPrice != null)
                expressions.Add(p => p.Price >= filter.MinPrice);
            if (filter.MaxPrice != null)
                expressions.Add(p => p.Price <= filter.MaxPrice);
            if (!String.IsNullOrEmpty(filter.Keyword))
                expressions.Add(p => p.Name.StartsWith(filter.Keyword));
            return expressions;
        }
        public IEnumerable<ProductDTO> GetProducts(int take = 0, int skip = 0, string sortBy = null, ProductFilterDTO filter = null)
        {
            var products = from p in _db.Products select p;
            var sortExpression = SortExpression(sortBy);
            if (sortExpression != null)
            {
                if (sortBy.Contains("asc"))
                    products = products.OrderBy(sortExpression);
                else
                    products = products.OrderByDescending(sortExpression);
            }

            if (filter != null)
            {
                var filterExpressions = FilterExpressions(filter);
                foreach (var expression in filterExpressions)
                    products = products.Where(expression);
            }
            if (skip > 0)
                products = products.Skip(skip);
            if (take > 0)
                products = products.Take(take);
            return products.Include(p => p.Reviews).Select(p => new ProductDTO(p, p.Reviews.Count() > 0 ? p.Reviews.Average(p => p.Score) : 0)).ToList();
        }
    }
}
