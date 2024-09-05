using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopDienThoai.Data;
using ShopDienThoai.Models;
using System.Security.Claims;
using static NuGet.Packaging.PackagingConstants;
using System.Net.Mail;
using System.Net;
using ShopDienThoai.Controllers;

namespace ShopDienThoai.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly ShopDienThoaiContext _context;
        private readonly IEmailService _emailService;
        private List<SelectListItem> roles = new List<SelectListItem>
        {
            new SelectListItem { Value = "QTV", Text = "Quản trị viên" },
            new SelectListItem { Value = "KH", Text = "Khách hàng", Selected = true }
        };
        public UsersController(ShopDienThoaiContext context, IEmailService emailService)
        {
            _emailService = emailService;
			_context = context;
        }

        // GET: Admin/Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
        }

        // GET: Admin/Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }
            ViewBag.Roles = new SelectList(roles, "Value", "Text", user.Role);
            return View(user);
        }

        // GET: Admin/Users/Create
        public IActionResult Create()
        {
            ViewBag.Roles = roles;
            return View();
        }

        // POST: Admin/Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Name,Password,Email,PhoneNumber,Address,Role")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return Redirect("/admin/users");
            }
            return View(user);
        }

        // GET: Admin/Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewBag.Roles = new SelectList(roles, "Value", "Text", user.Role);
            return View(user);
        }

        // POST: Admin/Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Name,Password,Email,PhoneNumber,Address,Role")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }
            var checkName = _context.User.Where(x => x.Name == user.Name).ToList().Count < 1;
            if (!checkName)
            {
                return Redirect("/admin/home/error");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect("/admin/users");
            }
            return View(user);
        }

        // GET: Admin/Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            ViewBag.Roles = new SelectList(roles, "Value", "Text", user.Role);

            return View(user);
        }

        // POST: Admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                _context.User.Remove(user);
            }

            await _context.SaveChangesAsync();
            return Redirect("/admin/users");
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.UserId == id);
        }
        [HttpGet]
        public IActionResult Login(string? email, string? password)
        {
            User us = new User { Email = email, Password = password };
            return View(us);
        }
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password, bool? rememberMe)
        {

            var obj = _context.User.Where(x => x.Email == email && x.Password == password && x.VerifyAccount == "").FirstOrDefault();

            if (obj != null)
            {
                var claims = new List<Claim>
                {
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
            return Redirect("/users/login");
        }


        public async Task<IActionResult> Logout()
        {

            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/admin/users/login");
        }



        // QUÊN MẬT KHẨU

        // GET: /Account/ForgotPassword: Hiển thị view để người dùng nhập email
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
            if (account != null)
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
                return RedirectToAction("errornotfound", "home");
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
