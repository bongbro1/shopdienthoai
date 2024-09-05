using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopDienThoai.Data;
using ShopDienThoai.Models;

namespace ShopDienThoai.Controllers
{
    public class UsersController : Controller
    {
        private readonly ShopDienThoaiContext _context;
		private readonly IEmailService _emailService;

		public UsersController(ShopDienThoaiContext context, IEmailService emailService)
        {
			_emailService = emailService;
			_context = context;
		}

        // GET: Users
        public async Task<IActionResult> Index()
        {
			if (User.Identity.IsAuthenticated)
			{
				var userId = int.Parse(User.FindFirstValue("Id"));
				var user = _context.User.Where(x => x.UserId == userId).FirstOrDefault();

				var wishlist = _context.WishList.Include(x => x.Product).Include(x => x.User).Where(x => x.UserId == userId).ToList();
				ViewBag.WishList = wishlist;
				return View(user);
			}
			else
			{
				return Redirect("/users/login");
			}
		}

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public IActionResult Edit(int id, string name, string email, string phone, string address)
        {
			User userOld = _context.User.Where(x => x.UserId == id).FirstOrDefault();
			if (userOld != null)
			{
				userOld.Email = email;
				userOld.Name = name;
				userOld.PhoneNumber = phone;
				userOld.Address = address;
				_context.Update(userOld);
				_context.SaveChanges();
				TempData["ModalType"] = "success";
				TempData["ModalTitle"] = "Thành công!";
				TempData["ModalMessage"] = "Dữ liệu đã được lưu thành công.";


				return Redirect("/users");
			}
			TempData["ModalType"] = "error";
			TempData["ModalTitle"] = "Thất bại!";
			TempData["ModalMessage"] = "Có lỗi xảy ra vui lòng thử lại.";
			return Redirect("/home/error");
			
        }



		public IActionResult Login()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Login(string email, string password, bool rememberMe)
		{

			var obj = _context.User.Where(x => x.Email == email && x.Password == password && x.VerifyAccount == "").FirstOrDefault();

			if (obj != null)
			{
				var claims = new List<Claim>
				{
					new Claim(ClaimTypes.Name, obj.Name),
					new Claim(ClaimTypes.Email, obj.Email),
					new Claim("Id", obj.UserId.ToString()),
					new Claim(ClaimTypes.Role, obj.Role),
				};

				var claimsIdentity = new ClaimsIdentity(
					claims, CookieAuthenticationDefaults.AuthenticationScheme);


				await HttpContext.SignInAsync(
					CookieAuthenticationDefaults.AuthenticationScheme,
					new ClaimsPrincipal(claimsIdentity));

				return Redirect("/");
			}
			string message = "Tài khoản, mật khẩu không hợp lệ!";
			ViewBag.Message = message;
			return View();
		}

		public async Task<IActionResult> Logout()
		{

			await HttpContext.SignOutAsync(
			  CookieAuthenticationDefaults.AuthenticationScheme);
			return Redirect("/");
		}

		public IActionResult Register()
		{
			return View();
		}


		[HttpPost]
		public IActionResult Register(string name, string email, string password, string repassword)
		{
			var user = _context.User.Where(u => u.Email == email).FirstOrDefault();
			bool checkEmail = user == null;
			if (ModelState.IsValid && password == repassword && checkEmail)
			{
				User us = new User
				{
					Name = name,
					Email = email,
					Password = password,
					Role = "KH",
					PhoneNumber = "",
					Address = ""
				};

				string resetCode = Guid.NewGuid().ToString();// tạo chuỗi code random để gửi kèm link reset password
				_emailService.SendVerificationLinkEmail(email, resetCode, "register", null);
				us.VerifyAccount = resetCode;
				_context.Add(us);
				_context.SaveChanges();
				ViewBag.Message = "Chúng tôi đã gửi email xác nhận tới bạn. Vui lòng kiểm tra để đăng nhập";
				ViewBag.Status = true;
				return Redirect("/users/register");
			}
			ViewBag.Status = false;
			if (checkEmail == false)
			{
				ViewBag.Message = "Email này đã đăng ký tài khoản vui lòng thử lại!";
			}
			else
			{
				ViewBag.Message = "Vui lòng nhập mật khẩu trùng nhau!";
			}
			return Redirect("/users/register");
		}

		[HttpGet]
		public IActionResult Register(string? id)
		{
			User us = _context.User.Where(x => x.VerifyAccount == id).FirstOrDefault();
			if (us != null)
			{
				if (id != null)
				{
					us.VerifyAccount = "";
					_context.SaveChanges();
					return View(us);
				}
			}
			if (id == null)
				return View();
			return RedirectToAction("errornotfound", "home");
		}


		public IActionResult ForgotPassword()
		{
			return View();
		}
		// POST: /Account/ForgotPassword: Nhận email từ form người dùng nhập

		[HttpPost]
		public IActionResult ForgotPassword(string EmailID)
		{
			//Verify Email ID
			//Generate Reset password link 
			//Send Email 
			string message = "";
			bool status = false;

			var account = _context.User.Where(a => a.Email == EmailID).FirstOrDefault();
			if (account != null && account.Role == "QTV")
			{
				//Send email for reset password
				string resetCode = Guid.NewGuid().ToString();// tạo chuỗi code random để gửi kèm link reset password
				_emailService.SendVerificationLinkEmail(account.Email, resetCode, "resetpassword", null);
				account.ResetPasswordCode = resetCode;
				//This line I have added here to avoid confirm password not match issue , as we had added a confirm password property 
				//in our model class in part 1
				_context.SaveChanges();
				message = "Reset password link has been sent to your email id.";
			}
			else
			{
				message = "Account not found";
			}

			ViewBag.Message = message;
			return View();
		}

		// lấy dữ liệu từ link người dùng bấm ở gmail. Id là chuỗi sau Users/ResetPassword/...
		public IActionResult ResetPassword(string id)
		{
			//Verify the reset password link
			//Find account associated with this link
			//redirect to reset password page
			if (string.IsNullOrWhiteSpace(id))
			{
				return Redirect("/home/error");
			}

			var user = _context.User.Where(a => a.ResetPasswordCode == id).FirstOrDefault();
			if (user != null)
			{
				ResetPasswordModel model = new ResetPasswordModel();
				model.Email = user.Email;
				model.ResetCode = id;
				return View(model);
			}
			else
			{
				return Redirect("/home/error");
			}
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult ResetPassword(ResetPasswordModel model)
		{
			var message = "";
			var status = false;
			if (ModelState.IsValid)
			{
				var user = _context.User.Where(a => a.ResetPasswordCode == model.ResetCode).FirstOrDefault();
				model.Email = user.Email;
				if (user != null)
				{
					user.Password = model.NewPassword;
					user.ResetPasswordCode = "";
					_context.SaveChanges();
					message = "New password updated successfully";
					status = true;
				}
			}
			else
			{
				message = "Something invalid";
				status = false;
			}
			ViewBag.Message = message;
			ViewBag.Status = status;
			return View(model);
		}
	}
}
