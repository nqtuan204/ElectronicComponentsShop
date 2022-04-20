using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ElectronicComponentsShop.Services.Cart;
using ElectronicComponentsShop.Services.User;
using ElectronicComponentsShop.DTOs;
using ElectronicComponentsShop.Services.Jwt;

namespace ElectronicComponentsShop.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartSv;
        private readonly IUserService _userSv;
        private readonly IJwtService _jwtSv;
        public CartController(ICartService cartSv, IUserService userSv, IJwtService jwtSv)
        {
            _cartSv = cartSv;
            _userSv = userSv;
            _jwtSv = jwtSv;
        }
        // GET: CartController
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        // GET: CartController/Details/5
        [Authorize]
        public async Task<IEnumerable<ItemDTO>> GetItems()
        {
            var userId = GetUserId();
            var items = await _cartSv.GetItems(userId);
            return items;
        }

        private int GetUserId()
        {
            var claims = _jwtSv.GetUserClaims(Request.Cookies["token"]);
            int userId = int.Parse(claims.First(c => c.Type == "Id").Value);
            return userId;
        }

        [Authorize]
        public async Task<IEnumerable<ItemDTO>> AddItem(int id,int quantity)
        {
            Console.WriteLine(id);
            Console.WriteLine(quantity);
            var userId = GetUserId();
            await _cartSv.AddItem(userId, id, quantity);
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
