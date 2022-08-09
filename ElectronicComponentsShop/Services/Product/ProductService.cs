using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ElectronicComponentsShop.Entities;
using ElectronicComponentsShop.DTOs;
using ElectronicComponentsShop.Database;
using Microsoft.EntityFrameworkCore;
using ElectronicComponentsShop.Models;

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
            if (!_db.Reviews.Any(r => r.ProductId == id))
                return 0;
            return _db.Reviews.Where(r => r.ProductId == id).Average(r => r.Score);
        }
        private Expression<Func<Entities.Product, dynamic>> SortExpression(string sortBy)
        {
            if (String.IsNullOrEmpty(sortBy))
                return null;
            if (sortBy.StartsWith("date"))
                return p => p.CreatedAt;
            if (sortBy.StartsWith("views"))
                return p => p.Views;
            if (sortBy.StartsWith("name"))
                return p => p.Name;
            if (sortBy.StartsWith("score"))
                return p => p.Reviews.Average(r => r.Score);
            if (sortBy.StartsWith("price"))
                return p => p.Price;
            return null;
        }

        private IEnumerable<Expression<Func<Entities.Product, bool>>> FilterExpressions(ProductFilterDTO filter)
        {
            var expressions = new List<Expression<Func<Entities.Product, bool>>>();
            if (filter == null)
                return expressions;
            if (filter.CategoryIds != null && filter.CategoryIds.Count > 0)
                expressions.Add(p => filter.CategoryIds.Contains(p.CategoryId));
            if (filter.MinPrice != null)
                expressions.Add(p => p.Price >= filter.MinPrice);
            if (filter.MaxPrice != null)
                expressions.Add(p => p.Price <= filter.MaxPrice);
            if (!String.IsNullOrEmpty(filter.Keyword))
            {
                filter.Keyword = filter.Keyword.ToLower();
                expressions.Add(p => p.Name.ToLower().StartsWith(filter.Keyword));
            }
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

        public int Count(ProductFilterDTO filter)
        {
            if (filter == null)
                return _db.Products.Count();
            var products = from p in _db.Products select p;
            var filterExpressions = FilterExpressions(filter);
            foreach (var expression in filterExpressions)
                products = products.Where(expression);
            return products.Count();
        }

        public ProductDetailsDTO GetProductDetails(int id)
        {
            var product = _db.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == id);
            if (product == null)
                return null;
            return new ProductDetailsDTO(product);
        }

        public IEnumerable<string> GetImageURLs(int id)
        {
            return _db.ProductImages.Where(i => i.ProductId == id).Select(i => i.URL);
        }

        public IEnumerable<ReviewDTO> GetAllReviews(int id)
        {
            return _db.Reviews.Include(r => r.User).Select(r => new ReviewDTO(r));
        }

        public int GetNumOfReviews(int id)
        {
            return _db.Reviews.Where(r => r.ProductId == id).Count();
        }

        public Dictionary<int, int> GetScoreStats(int id)
        {
            return _db.Reviews.Where(r => r.ProductId == id).GroupBy(r => r.Score).Select(g => new { Score = g.Key, NumberOfReviews = g.Count() }).ToDictionary(a => a.Score, a => a.NumberOfReviews);
        }

        public IEnumerable<ReviewDTO> GetPagedReviews(int id, int page)
        {
            return _db.Reviews.Include(r => r.User).Where(r => r.ProductId == id).OrderByDescending(r => r.CreatedAt).Select(r => new ReviewDTO(r)).Skip(5 * (page - 1)).Take(5);
        }

        public IEnumerable<ProductDTO> GetRelatedProducts(int id, int categoryId)
        {
            var avgScore = GetAverageScore(id);
            var products = _db.Products.Where(p => p.CategoryId == categoryId).Where(p => p.Id != id);
            var r = new Random();
            int skip = r.Next(1, products.Count() - 7);
            return products.OrderBy(p => p.Name).Skip(skip).Take(6).Select(p => new ProductDTO(p, avgScore));
        }

        public async Task CreateReview(NewReviewDTO newReview)
        {
            Review review = new(newReview);
            _db.Reviews.Add(review);
            await _db.SaveChangesAsync();
        }

        public int CountProducts(string keyword, int categoryId)
        {
            var products = from p in _db.Products select p;
            if (!String.IsNullOrEmpty(keyword))
                products = products.Where(p => p.Name.StartsWith(keyword));
            if (categoryId > 0 && categoryId <= 10)
                products = products.Where(p => p.CategoryId == categoryId);
            return products.Count();
        }

        public IList<ProductDataVM> GetProductsData(string sortBy, string keyword, int categoryId, int page)
        {
            var products = (from p in _db.Products select p).Include(p => p.Category).AsQueryable();

            if (!String.IsNullOrEmpty(sortBy))
            {
                if (sortBy.Contains("asc"))
                {
                    products = products.OrderBy(p => p.CreatedAt);
                    if (sortBy.Contains("productId"))
                        products = products.OrderBy(p => p.Id);
                    if (sortBy.Contains("name"))
                        products = products.OrderBy(p => p.ModifiedAt);
                    if (sortBy.Contains("category"))
                        products = products.OrderBy(p => p.Category.Name);
                    if (sortBy.Contains("price"))
                        products = products.OrderBy(p => p.Price);
                }
                else
                {
                    products = products.OrderByDescending(p => p.CreatedAt);
                    if (sortBy.Contains("productId"))
                        products = products.OrderByDescending(p => p.Id);
                    if (sortBy.Contains("name"))
                        products = products.OrderByDescending(p => p.ModifiedAt);
                    if (sortBy.Contains("category"))
                        products = products.OrderByDescending(p => p.Category.Name);
                    if (sortBy.Contains("price"))
                        products = products.OrderByDescending(p => p.Price);
                }

            }

            if (!String.IsNullOrEmpty(keyword))
                products = _db.Products.Where(p => p.Name.StartsWith(keyword));
            if (categoryId > 0 && categoryId <= 10)
                products = products.Where(p => p.CategoryId == categoryId);
            Console.WriteLine(products.Count());
            return products.Include(p=>p.Category).Select(p => new ProductDataVM(p)).Skip((page - 1) * 30).Take(30).ToList();

        }
    }
}
