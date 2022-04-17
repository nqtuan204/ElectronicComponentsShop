using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ElectronicComponentsShop.Services.Cart;
using ElectronicComponentsShop.DTOs;
using System.IdentityModel.Tokens.Jwt;

namespace ElectronicComponentsShop.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartSv;
        public CartController(ICartService cartSv)
        {
            _cartSv = cartSv;
        }
        // GET: CartController
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        // GET: CartController/Details/5
        [Authorize]
        public async Task<IEnumerable<CartItemDTO>> GetItems()
        {
            var userId = GetUserId();
            var items = await _cartSv.GetItems(userId);
            return items;
        }

        private int GetUserId()
        {
            var token = new JwtSecurityTokenHandler().ReadJwtToken(HttpContext.Request.Cookies["token"]);
            int userId = int.Parse(token.Claims.First(c => c.Type == "Id").Value);
            return userId;
        }

        [Authorize]
        public async Task<IEnumerable<CartItemDTO>> AddItem(int id)
        {
            var userId = GetUserId();
            await _cartSv.AddItem(userId, id);
            var items = await _cartSv.GetItems(userId);
            return items;
        }

        
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Update([FromBody] CartDTO cart)
        {
            var userId = GetUserId();
            await _cartSv.Update(userId, cart.Items);
            return Ok(cart.Items);
        }

        [Authorize]
        public async Task<ActionResult> RemoveAll(int id)
        {
            var userId = GetUserId();
            await _cartSv.RemoveAll(userId, id);
            return Ok();
        }

        [Authorize]
        public async Task<ActionResult> Clear()
        {
            var userId = GetUserId();
            await _cartSv.Clear(userId);
            return Ok();
        }

        // GET: CartController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CartController/Create
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

        // GET: CartController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CartController/Edit/5
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

        // GET: CartController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CartController/Delete/5
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
