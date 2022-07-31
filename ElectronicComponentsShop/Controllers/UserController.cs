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
using ElectronicComponentsShop.Services.Email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace ElectronicComponentsShop.Controllers
{
    public class UserController : Controller
    {
        private readonly ECSDbContext _db;
        private readonly IUserService _userSv;
        private readonly IJwtService _jwt;
        private readonly IEmailService _emailSv;
        private readonly IConfiguration _config;
        public UserController(ECSDbContext db, IUserService userSv, IJwtService jwt, IEmailService emailSv, IConfiguration config)
        {
            _db = db;
            _userSv = userSv;
            _jwt = jwt;
            _emailSv = emailSv;
            _config = config;
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
            ViewBag.Title = "Đăng ký";
            ViewBag.BCTree = new Dictionary<string, string> { { "Trang chủ", "/" }, { "Đăng ký", "/User/Register" } };
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
            ViewBag.Title = "Đăng nhập";
            ViewBag.BCTree = new Dictionary<string, string> { { "Trang chủ", "/" }, { "Đăng nhập", "/User/Login" } };
            if (HttpContext.Request.Cookies.Keys.Contains("token"))
                return Redirect("/");
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginUserVM user)
        {
            var dto = _userSv.GetLoginUser(user.PhoneNumberOrEmail, user.Password);
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
            int userId = GetUserId();
            var favProductIds = _userSv.GetFavProducts(userId).Select(p => p.Id);
            if (!favProductIds.Contains(id))
                await _userSv.AddToFavourites(userId, id);
            else
                await _userSv.RemoveFromFavourites(userId, id);

            return Ok(id);
        }

        [Authorize]
        public async Task<IActionResult> RemoveFromFavourites(int id)
        {
            int userId = GetUserId();
            var favProductIds = _userSv.GetFavProducts(userId).Select(p => p.Id);
            if (favProductIds.Contains(id))
                await _userSv.RemoveFromFavourites(userId, id);
            else
                await _userSv.AddToFavourites(userId, id);
            return Ok(id);
        }

        [Authorize]
        public ActionResult GetFavouriteProductIds()
        {
            int userId = GetUserId();
            var favProductIds = _userSv.GetFavProducts(userId).Select(p => p.Id);
            return Json(favProductIds);
        }

        [Authorize]
        public ActionResult GetFavouriteProducts()
        {
            int userId = GetUserId();
            var favProducts = _userSv.GetFavProducts(userId);
            return Json(favProducts);
        }

        [Authorize]
        public ActionResult Profile()
        {
            return View();
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ResetPassword(ResetPasswordVM resetPasswordVM)
        {
            Console.WriteLine("Require reset password");
            if (!ModelState.IsValid)
                return View("ForgotPassword", resetPasswordVM);
            var userId = _userSv.GetUserByEmail(resetPasswordVM.Email).Id;
            string token = await _userSv.GenerateResetPasswordToken(userId);
            string url = _config["URL"] + "/User/ConfirmResetPassword?token=" + token;
            string title = "[linhkiendientu204] Yêu cầu thiết lập lại mật khẩu";
            string content = $"Ai đó đã yêu cầu thiết lập lại mật khẩu bằng email của bạn, nếu đó là bạn, vui lòng nhấn vào liên kết sau để xác nhận thiết lập lại mật khẩu: " + url;
            _emailSv.Send(title, content, resetPasswordVM.Email);
            return View("ResetPassword", resetPasswordVM.Email);
        }

        public async Task<ActionResult> ConfirmResetPassword(string token)
        {
            token = Request.QueryString.Value.Replace("?token=", "");
            Console.WriteLine(token);
            if (_userSv.IsResetPasswordTokenNullOrExpire(token))
                return View("ExpiredResetPasswordToken");

            var user = _userSv.GetUserByResetPasswordToken(token);
            string newPassword = await _userSv.ResetPassword(user.Id);
            string title = "Thiết lập lại mật khẩu";
            string content = $"Bạn đã xác nhận thiết lập lại mật khẩu. Mật khẩu mới của bạn là: {newPassword}";
            _emailSv.Send(title, content, user.Email);
            return View("ConfirmResetPassword", user.Email);
        }


        public IActionResult IsExist(string Email)
        {
            Console.WriteLine("Check if email exist!");
            return Json(_userSv.IsEmailExist(Email));
        }

        public ActionResult GetUserInfoPartial()
        {
            var userId = GetUserId();
            var userInfo = new UserInfoVM(_userSv.GetUserById(userId));
            return PartialView("_UserInfo", userInfo);
        }

        public ActionResult GetChangePasswordPartial()
        {
            return new ObjectResult(null);
        }

        public ActionResult GetUserOrdersPartial()
        {
            return new ObjectResult(null);
        }

        public ActionResult GetFavProducts()
        {
            return new ObjectResult(null);
        }

        public ActionResult UpdateUserInfo(UserDTO dto)
        {
            var userId = GetUserId();
            dto.Id = userId;
            _userSv.Update(dto);
            return StatusCode(200);
        }

        public ActionResult ChangePassword(string password, string newPassword)
        {
            var userId = GetUserId();
            var user = _userSv.GetUserById(userId);
            if (_userSv.IsPasswordMatch(userId, password))
            {
                _userSv.ChangePassword(userId, newPassword);
                return StatusCode(200);
            }
            return StatusCode(404);
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
