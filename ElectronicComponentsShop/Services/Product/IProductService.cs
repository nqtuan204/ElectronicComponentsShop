using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicComponentsShop.DTOs;
using ElectronicComponentsShop.Models;

namespace ElectronicComponentsShop.Services.Product
{
    public interface IProductService
    {
        ProductDTO GetProduct(int id);
        double GetAverageScore(int id);
        IEnumerable<ProductDTO> GetProducts(int take = 0, int skip = 0, string sortBy = null, ProductFilterDTO filter = null);
        int Count(ProductFilterDTO filter);
        ProductDetailsDTO GetProductDetails(int id);
        IEnumerable<string> GetImageURLs(int id);
        Dictionary<int, int> GetScoreStats(int id);
        IEnumerable<ReviewDTO> GetAllReviews(int id);
        int GetNumOfReviews(int id);
        IEnumerable<ReviewDTO> GetPagedReviews(int id, int page);
        IEnumerable<ProductDTO> GetRelatedProducts(int id, int categoryId);
        Task CreateReview(NewReviewDTO newReview);
        int CountProducts(string keyword, int categoryId);
        IList<ProductDataVM> GetProductsData(string sortBy, string keyword, int categoryId, int page);
    }
}
