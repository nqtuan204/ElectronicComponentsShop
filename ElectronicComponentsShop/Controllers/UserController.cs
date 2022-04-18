using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicComponentsShop.Models;
using ElectronicComponentsShop.DTOs;
using ElectronicComponentsShop.Database;
using ElectronicComponentsShop.Services.User;
using ElectronicComponentsShop.Services.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace ElectronicComponentsShop.Controllers
{
    public class UserController : Controller
    {
        private readonly ECSDbContext _db;
        private readonly IUserService _userSv;
        private readonly IJwtService _jwt;

        public UserController(ECSDbContext db, IUserService userSv, IJwtService jwt)
        {
            _db = db;
            _userSv = userSv;
            _jwt = jwt;
        }

        [HttpPost]
        public IActionResult IsEmailExist(string Email)
        {
            return Json(!_userSv.IsEmailExist(Email));
        }
        [HttpPost]
        public IActionResult IsPhoneNumberExist(string PhoneNumber)
        {
            return Json(!_userSv.IsPhoneNumberExist(PhoneNumber));
        }
        // GET: UserController
        public ActionResult Register()
        {
            if (HttpContext.Request.Cookies.Keys.Contains("token"))
                return Redirect("/");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(NewUserVM user)
        {
            if (ModelState.IsValid)
            {
                await _userSv.Add(new NewUserDTO(user));
                return RedirectToAction("Login");
            }
            return View(user);
        }

        public IActionResult Login()
        {
            if (HttpContext.Request.Cookies.Keys.Contains("token"))
                return Redirect("/");
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginUserVM user)
        {
            var dto = _userSv.GetUser(user.PhoneNumberOrEmail, user.Password);
            if (dto == null)
            {
                ModelState.AddModelError("PhoneNumberOrEmail", "Tài khoản hoặc mật khẩu không đúng.");
                return View(user);
            }
            string token = _jwt.GetToken(dto);
            HttpContext.Response.Cookies.Append("token", token);
            return Redirect("/");
        }

        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("token");
            return RedirectToAction("Login");
        }

        private int GetUserId()
        {
            return int.Parse(_jwt.GetUserClaims(HttpContext.Request.Cookies["token"]).First(c => c.Type == "Id").Value);
        }

        [Authorize]
        public async Task<IActionResult> AddToFavourites(int id)
        {
            Console.WriteLine(id);
            int userId = GetUserId();
            if (!_userSv.GetFavProductIds(userId).Contains(id))
                await _userSv.AddToFavourites(userId, id);
            else
                await _userSv.RemoveFromFavourites(userId, id);

            return Ok(id);
        }

        [Authorize]
        public async Task<IActionResult> RemoveFromFavourites(int id)
        {
            int userId = GetUserId();
            if (_userSv.GetFavProductIds(userId).Contains(id))
                await _userSv.RemoveFromFavourites(userId, id);
            else
                await _userSv.AddToFavourites(userId, id);
            return Ok(id);
        }

        [Authorize]
        public ActionResult GetFavouriteProductIds()
        {
            int userId = GetUserId();
            return Json(_userSv.GetFavProductIds(userId));
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
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

        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController/Edit/5
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

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
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
