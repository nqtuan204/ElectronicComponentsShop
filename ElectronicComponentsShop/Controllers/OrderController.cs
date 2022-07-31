using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicComponentsShop.Services.Cart;
using ElectronicComponentsShop.Services.Jwt;
using ElectronicComponentsShop.Services.User;
using ElectronicComponentsShop.Services.Order;
using ElectronicComponentsShop.DTOs;
using ElectronicComponentsShop.Models;
using Microsoft.AspNetCore.Authorization;

namespace ElectronicComponentsShop.Controllers
{
    public class OrderController : Controller
    {
        private readonly IUserService _userSv;
        private readonly IJwtService _jwtSv;
        private readonly ICartService _cartSv;
        private readonly IOrderService _orderSv;

        private int GetUserId()
        {
            string token = Request.Cookies["token"];
            var claims = _jwtSv.GetUserClaims(token);
            int userId = int.Parse(claims.First(c => c.Type == "Id").Value);
            return userId;
        }

        public OrderController(IUserService userSv, IJwtService jwtSv, ICartService cartSv, IOrderService orderSv)
        {
            _userSv = userSv;
            _jwtSv = jwtSv;
            _cartSv = cartSv;
            _orderSv = orderSv;
        }

        [Authorize]
        // GET: OrderController
        public async Task<ActionResult> Checkout()
        {
            int userId = GetUserId();
            var user = _userSv.GetUserById(userId);
            var paymentTypes = _orderSv.GetAllPaymentTypes();
            var items = await _cartSv.GetItems(userId);
            if (!items.Any())
                return Redirect("/Cart");
            decimal amount = items.Sum(item => item.Quantity * item.Price);
            var vm = new CheckoutVM(user, items.Select(i => new ItemVM(i)), paymentTypes, amount);
            return View(vm);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Checkout(CheckoutVM checkout)
        {
            if (!ModelState.IsValid)
                return View(checkout);
            var userId = GetUserId();
            var newOrder = new NewOrderDTO(userId, checkout);
            await _orderSv.CreateOrder(newOrder);
            await _cartSv.Clear(userId);
            return Redirect("/");
        }

        public ActionResult GetUserOrdersPartial([FromQuery] int page = 1, [FromQuery] int orderStateId = 0)
        {
            var userId = GetUserId();
            var userOrders = _orderSv.GetUserOrders(userId, page, orderStateId);
            return PartialView("_UserOrders", userOrders);
        }
        // GET: OrderController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: OrderController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OrderController/Create
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

        // GET: OrderController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OrderController/Edit/5
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

        // GET: OrderController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OrderController/Delete/5
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
