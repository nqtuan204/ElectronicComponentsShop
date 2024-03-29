﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicComponentsShop.Models;
using ElectronicComponentsShop.DTOs;
using ElectronicComponentsShop.Services.Product;
using ElectronicComponentsShop.Services.Category;
using ElectronicComponentsShop.Services.Jwt;

namespace ElectronicComponentsShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productSv;
        private readonly ICategoryService _categorySv;
        private readonly IJwtService _jwtSv;
        public ProductController(IProductService productSv, ICategoryService categorySv, IJwtService jwtSv)
        {
            _productSv = productSv;
            _categorySv = categorySv;
            _jwtSv = jwtSv;
        }
        // GET: ProductController
        public ActionResult List(int page = 1, int pageSize = 9, string sortBy = null, ProductFilterVM filter = null)
        {
            ViewBag.Title = "Danh sách sản phẩm";
            ViewBag.BCTree = new Dictionary<string, string> { { "Trang chủ", "/" }, { "Danh sách sản phẩm", "/Product/List" } };
            if (!String.IsNullOrEmpty(filter.Keyword))
                return RedirectToAction("Search");
            var filterDTO = new ProductFilterDTO(filter);
            var categories = _categorySv.GetCategories().Select(c => new CategoryVM(c));
            var productListVM = new ProductListVM(page, pageSize, filter, sortBy, categories);
            return View(productListVM);
        }

        public ActionResult<int> Count(ProductFilterVM filter = null)
        {
            var filterDTO = new ProductFilterDTO(filter);
            return _productSv.Count(filterDTO);
        }

        public ActionResult GetProductGridPartial(int page = 1, int pageSize = 9, string sortBy = null, ProductFilterVM filter = null)
        {
            var filterDTO = new ProductFilterDTO(filter);
            var products = _productSv.GetProducts(pageSize, pageSize * (page - 1), sortBy, filterDTO).Select(p => new ProductVM(p));
            return PartialView("_ProductGrid", products);
        }

        public ActionResult Search(int page = 1, int pageSize = 9, string sortBy = null, ProductFilterVM filter = null)
        {
            ViewBag.Title = "Tìm kiếm";
            ViewBag.BCTree = new Dictionary<string, string> { { "Trang chủ", "/" }, { "Danh sách sản phẩm", "/Product/Search" } };
            if (String.IsNullOrEmpty(filter.Keyword))
                return RedirectToAction("List");
            ViewBag.keyword = filter.Keyword;
            var filterDTO = new ProductFilterDTO(filter);
            var categories = _categorySv.GetCategories().Select(c => new CategoryVM(c));
            var productListVM = new ProductListVM(page, pageSize, filter, sortBy, categories);
            return View("List", productListVM);
        }

        // GET: ProductController/Details/5
        public ActionResult Details(int id, string slug)
        {
            var dto = _productSv.GetProductDetails(id);
            if (dto.Slug != slug)
                return Redirect($"/Product/{dto.Id}.{dto.Slug}");
            double avgScore = _productSv.GetAverageScore(id);
            var imageURLs = _productSv.GetImageURLs(id);
            var numOfReviews = _productSv.GetNumOfReviews(id);
            var details = new ProductDetailsVM(dto, imageURLs, avgScore, numOfReviews);
            var relatedProducts = _productSv.GetRelatedProducts(id, dto.CategoryId).Select(p => new ProductVM(p));
            ViewBag.RelatedProductCarousel = new ProductCarouselVM("Sản phẩm cùng danh mục", relatedProducts, 1);
            ViewBag.Title = dto.Name;
            ViewBag.BCTree = new Dictionary<string, string> { { "Trang chủ", "/" }, { "Danh sách sản phẩm", "/Product/List" }, { dto.Name, $"/Product/{dto.Id}.{dto.Slug}" } };
            return View(details);
        }

        public ActionResult GetRatingPartial(int id)
        {
            var rating = _productSv.GetScoreStats(id);
            return PartialView("_Rating", rating);
        }

        public ActionResult GetReviewsPartial(int id, int page = 1)
        {
            var reviews = _productSv.GetPagedReviews(id, page).Select(r => new ReviewVM(r));
            return PartialView("_Reviews", reviews);
        }

        public ActionResult GetProductCarouselPartial(string title)
        {
            if (title == "newest")
            {
                var newestProducts = _productSv.GetProducts(6, 0, "date_desc").Select(p => new ProductVM(p));
                return PartialView("_ProductCarousel", new ProductCarouselVM("Sản phẩm mới", newestProducts, 1));
            }
            if (title == "mostviewed")
            {
                var mostViewedProducts = _productSv.GetProducts(6, 0, "views_desc").Select(p => new ProductVM(p));
                return PartialView("_ProductCarousel", new ProductCarouselVM("Xem nhiều", mostViewedProducts, 2));
            }
            return Ok();
        }

        public ActionResult GetRelatedProductCarouselPartial(int id, int categoryId)
        {
            var relatedProducts = _productSv.GetRelatedProducts(id, categoryId).Select(p => new ProductVM(p));
            return PartialView("_ProductCarousel", new ProductCarouselVM("Sản phẩm cùng danh mục", relatedProducts, 3));
        }
        private int? GetUserId()
        {
            try
            {
                var claims = _jwtSv.GetUserClaims(Request.Cookies["token"]);
                int userId = int.Parse(claims.First(c => c.Type == "Id").Value);
                return userId;
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateReview([FromBody] NewReviewVM newReview)
        {
            var userId = GetUserId();
            NewReviewDTO dto = new(newReview, userId);
            await _productSv.CreateReview(dto);
            return StatusCode(200);
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
