using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using ElectronicComponentsShop.Models;
using ElectronicComponentsShop.Services.Order;
using ElectronicComponentsShop.Services.User;
using ElectronicComponentsShop.Services.Product;
using ElectronicComponentsShop.Services.Stat;
using ElectronicComponentsShop.Services.Category;
using ElectronicComponentsShop.DTOs;
using Microsoft.AspNetCore.Http;

namespace ElectronicComponentsShop.Controllers
{
    public class AdminController : Controller
    {
        private readonly Database.ECSDbContext _db;
        private readonly IOrderService _orderSv;
        private readonly IUserService _userService;
        private readonly IProductService _productSv;
        private readonly IStatService _statSv;
        private readonly ICategoryService _categorySv;

        public AdminController(Database.ECSDbContext db, IOrderService orderSv, IUserService userService, IProductService productSv, IStatService statSv, ICategoryService categorySv)
        {
            _db = db;
            _orderSv = orderSv;
            _userService = userService;
            _productSv = productSv;
            _statSv = statSv;
            _categorySv = categorySv;
        }

        [Authorize(policy: "OnlyAdmin")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateProduct()
        {
            ViewBag.Categories = _categorySv.GetCategories();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] IFormFile Thumbnail, NewProduct newProduct)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", Thumbnail.FileName);
            using (var fs = new FileStream(path, FileMode.Create))
            {
                await Thumbnail.CopyToAsync(fs);
            }
            newProduct.ThumbnailURL = $"/images/{Thumbnail.FileName}";
            await _productSv.CreateProduct(newProduct);
            ViewBag.Categories = _categorySv.GetCategories();
            return View();
        }

        [Authorize(policy: "OnlyAdmin")]
        [HttpPost]
        public IActionResult GetQuantityNotificationPartial(string title, DateTime from, DateTime to)
        {
            string quantity = _userService.CountNewUsers(from, to).ToString();
            string Title = "Người dùng mới";
            if (title == "orders")
            {
                quantity = _orderSv.CountNewOrders().ToString();
                Title = "Đơn hàng mới";
            }
            if (title == "completed-orders")
            {
                quantity = _orderSv.CountCompletedOrders(from, to).ToString();
                Title = "Đơn đã giao";
            }
            if (title == "revenue")
            {
                quantity = _statSv.GetRevenue(from, to).ToString("0,0") + "đ";
                Title = "Doanh thu";
            }
            return PartialView("_QuantityNotification", new QuantityNotificationVM(Title, quantity));
        }

        [Authorize(policy: "OnlyAdmin")]
        [HttpPost]
        public IActionResult GetRevenueStat(DateTime from, DateTime to)
        {
            return Json(_statSv.GetRevenueStat(from, to));
        }

        [Authorize(policy: "OnlyAdmin")]
        [HttpPost]
        public IActionResult GetRevenueStatByLocal(DateTime from, DateTime to)
        {
            return Json(_statSv.GetRevenueStatByLocal(from, to));
        }

        [Authorize(policy: "OnlyAdmin")]
        [HttpPost]
        public IActionResult GetTopStatPartial(string title, DateTime from, DateTime to)
        {
            TopStatVM top = new()
            {
                Title = "Khách mua nhiều",
                TopList = _statSv.GetTopCustomers(from, to)
            };
            if (title == "products")
            {
                top = new()
                {
                    Title = "Sản phẩm bán chạy",
                    TopList = _statSv.GetTopProducts(from, to)
                };
            }
            return PartialView("_TopStat", top);
        }

        [Authorize(policy: "OnlyAdmin")]
        public IActionResult GetCategoriesStat(DateTime from, DateTime to)
        {
            var stat = _statSv.GetCategoriesStat(from, to);
            return Json(stat);
        }

        [Authorize(policy: "OnlyAdmin")]
        private IEnumerable<string> GenQueries(DateTime from, DateTime to)
        {
            List<string> queries = new();
            using (var wt = new StreamWriter(@"C:\Users\Admin\Desktop\orders.txt"))
            {
                var rd = new Random();
                DateTime date = from;
                var query = "";
                int orderId = 6526;
                while (date <= to)
                {
                    var amount = 0;
                    var limitAmount = rd.Next(7, 16);

                    do
                    {
                        orderId++;
                        limitAmount = rd.Next(7, 16);

                        date = new DateTime(date.Year, date.Month, date.Day, rd.Next(1, 24), rd.Next(1, 60), rd.Next(1, 60));
                        var lastmodified = date.AddDays(rd.Next(1, 4));
                        var modifiedAt = new DateTime(lastmodified.Year, lastmodified.Month, lastmodified.Day, rd.Next(1, 24), rd.Next(1, 60), rd.Next(1, 60));
                        //var status = rd.Next(1, 100) == 1 ? 2 : 4;
                        var status = 1;
                        int wardcount = _db.Wards.Count() + 1;
                        int wardId = rd.Next(1, wardcount);
                        var ward = _db.Wards.Include(w => w.District).First(w => w.Id == wardId);
                        //query = $"INSERT INTO \"Orders\"(\"CreatedAt\",\"ModifiedAt\",\"Address\",\"OrderStateId\",\"UserId\",\"Note\",\"PaymentTypeId\",\"DistrictId\",\"ProvinceId\",\"WardId\") VALUES('{date.Year}-{date.Month}-{date.Day} {date.Hour}:{date.Minute}:{date.Second}','{modifiedAt.Year}-{modifiedAt.Month}-{modifiedAt.Day} {modifiedAt.Hour}:{modifiedAt.Minute}:{modifiedAt.Second}','{rd.Next(100, 700)}',{status},{rd.Next(1, 10001)},null,1,{ward.DistrictId},{ward.District.ProvinceId},{ward.DistrictId});";
                        query = $"INSERT INTO \"Orders\"(\"CreatedAt\",\"ModifiedAt\",\"Address\",\"OrderStateId\",\"UserId\",\"Note\",\"PaymentTypeId\",\"DistrictId\",\"ProvinceId\",\"WardId\") VALUES('{date.Year}-{date.Month}-{date.Day} {date.Hour}:{date.Minute}:{date.Second}',null,'{rd.Next(100, 700)}',{status},{rd.Next(1, 10001)},null,1,{ward.DistrictId},{ward.District.ProvinceId},{ward.DistrictId});";
                        queries.Add(query);
                        int maxOrderAmount = rd.Next(1, 72);
                        int maxQuantity = rd.Next(1, 7);
                        int orderAmount = 0, orderQuantity = 0;
                        List<int> selectedProducts = new();
                        while (orderAmount < maxOrderAmount * 100000 && orderQuantity < maxQuantity)
                        {
                            orderQuantity++;
                            int productId = rd.Next(1, 3241);
                            while (selectedProducts.Contains(productId))
                                productId = rd.Next(1, 3241);
                            selectedProducts.Add(productId);
                            var product = _db.Products.Find(productId);
                            int quantity = rd.Next(1, 7);
                            //if (status == 4)
                            amount += (int)product.Price * quantity;
                            query = $"INSERT INTO \"OrderItems\"(\"OrderId\",\"ProductId\",\"Quantity\",\"Price\") VALUES({orderId},{productId},{quantity},{product.Price});";
                            queries.Add(query);
                        }

                    } while (amount < limitAmount * Math.Pow(10, 6));

                    date = date.AddDays(1);
                }
                return queries;
            }
        }

        [Authorize(policy: "OnlyAdmin")]
        public IActionResult OrderManagement()
        {
            return View();
        }

        [Authorize(policy: "OnlyAdmin")]
        public IActionResult ProductManagement()
        {
            return View();
        }

        [Authorize(policy: "OnlyAdmin")]
        [HttpPost]
        public IActionResult GetOrderTablePartial(string sortBy = "createdAt desc", string keyword = "", int orderStateId = 0, int page = 1)
        {
            ViewBag.sortBy = sortBy;
            ViewBag.keyword = keyword;
            ViewBag.orderStateId = orderStateId;
            ViewBag.total = _orderSv.CountOrders(keyword, orderStateId);
            ViewBag.page = page;
            var orders = _orderSv.GetOrders(sortBy, keyword, orderStateId, page);
            return PartialView("_OrderTable", orders);
        }

        [Authorize(policy: "OnlyAdmin")]
        public async Task<IActionResult> ChangeOrderState(int orderId, int orderStateId)
        {
            await _orderSv.ChangeOrderState(orderId, orderStateId);
            return StatusCode(200);
        }

        [Authorize(policy: "OnlyAdmin")]
        [HttpPost]
        public IActionResult GetProductTablePartial(string sortBy = "createdAt desc", string keyword = "", int categoryId = 0, int page = 1)
        {
            ViewBag.sortBy = sortBy;
            ViewBag.keyword = keyword;
            ViewBag.orderStateId = categoryId;
            ViewBag.total = _productSv.CountProducts(keyword, categoryId);
            ViewBag.page = page;
            var products = _productSv.GetProductsData(sortBy, keyword, categoryId, page);
            return PartialView("_ProductTable", products);
        }

        [Authorize(policy: "OnlyAdmin")]
        public IActionResult GenOrders()
        {
            var queries = GenQueries(new DateTime(2022, 8, 1), new DateTime(2022, 8, 5));
            return View(queries);
        }


    }
}
