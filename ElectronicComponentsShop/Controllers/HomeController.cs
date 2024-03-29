﻿using ElectronicComponentsShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ElectronicComponentsShop.Services.Category;
using ElectronicComponentsShop.Services.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;

namespace ElectronicComponentsShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICategoryService _categorySv;
        private readonly IProductService _productSv;
        private readonly IMemoryCache _cache;

        public HomeController(ILogger<HomeController> logger, ICategoryService categorySv, IProductService productSv, IMemoryCache cache)
        {
            _logger = logger;
            _categorySv = categorySv;
            _productSv = productSv;
            _cache = cache;
        }
        
        public IActionResult Index()
        {
            IEnumerable<CategoryVM> categories;
            ProductCarouselVM[] productCarousels = new ProductCarouselVM[] { };
            if (!_cache.TryGetValue("categories",out categories) && !_cache.TryGetValue("productCarousels", out productCarousels))
            {
                categories = _categorySv.GetCategories().Select(c => new CategoryVM(c));
                var newestProducts = _productSv.GetProducts(6, 0, "date_desc").Select(p => new ProductVM(p));
                var mostViewedProducts = _productSv.GetProducts(6, 0, "views_desc").Select(p => new ProductVM(p));
                productCarousels = new ProductCarouselVM[]{
                    new ProductCarouselVM("Sản phẩm mới", newestProducts,1),
                    new ProductCarouselVM("Xem nhiều",mostViewedProducts,2)
                };
            }
            var Home = new HomeVM(categories, productCarousels);
            return View(Home);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
