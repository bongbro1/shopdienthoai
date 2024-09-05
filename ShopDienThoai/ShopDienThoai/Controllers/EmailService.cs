using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopDienThoai.Models;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopDienThoai.Data;

namespace ShopDienThoai.Controllers
{
    public interface IEmailService
	{
		public void SendVerificationLinkEmail(string emailID, string activationCode, string emailFor, Order? order);
	}
    public class EmailService : Controller, IEmailService
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public EmailService(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public void SendVerificationLinkEmail(string emailID, string activationCode, string emailFor, Order? order)
		{
			var request = _httpContextAccessor.HttpContext?.Request;
			var link = "";
			var fromEmail = new MailAddress("bongtham01@gmail.com", "BONG BABY");
			var verifyUrl = "/users/" + emailFor + "/" + activationCode;

			var toEmail = new MailAddress(emailID);
			var fromEmailPassword = "mqfq swfw kfwt jwaa"; // mã xác thực 2 bước
			if (emailFor != "neworder")
			{
				var uriBuilder = new UriBuilder
				{
					Scheme = request.Scheme, // Lấy scheme của yêu cầu (http hoặc https)
					Host = request.Host.Host, // Lấy hostname của yêu cầu (localhost hoặc tên miền)
					Path = verifyUrl
				};
				// Chỉ đặt port nếu port không phải là null
				if (request.Host.Port.HasValue)
				{
					uriBuilder.Port = request.Host.Port.Value;
				}
				link = uriBuilder.Uri.AbsoluteUri;
			}

			string subject = "";
			string body = "";
			if (emailFor == "register")
			{
				subject = "Your account is successfully created!";
				body = "<br/><br/>We are excited to tell you that your ShopDienThoai account is" +
						" successfully created. Please click on the below link to verify your account" +
						" <br/><br/><a href='" + link + "'>" + link + "</a> ";
			}
			else if (emailFor == "resetpassword")
			{
				subject = "reset password";
				body = "Hi,<br/><br/>We got request for reset your account password. Please click on the below link to reset your password" +
						"<br/><br/><a href=" + link + ">Reset Password link</a>";
			}
			//    else if (emailFor == "neworder")
			//    {
			//        user us = GetUserByUserId(ordered.UserId);
			//        Orders order = GetOrderByOrderId(ordered.OrdersId);
			//        List<customerOrder> listItemOrdered = _context.customerOrder.Where(x => x.OrderedId == ordered.Id && x.OrderStatus == 1 && x.UserId == us.Id).ToList();
			//        subject = "New Order";
			//        string s = "";
			//        foreach (var item in listItemOrdered)
			//        {
			//            product pr = _productService.Value.GetProductByProductId(item.ProductId);
			//            s += "   - <b>" + pr.Name.ToString() + " (x" + item.Count + ")" + "</b><br/>";
			//        }
			//        body = "Kính gửi <b>Quản trị viên</b>,<br/><br/>" +
			//"Tôi viết thư này để thông báo rằng chúng tôi đã nhận được một đơn hàng mới trên trang web của chúng ta. Chi tiết đơn hàng như sau: <br/>" +
			//"Mã đơn hàng: <b>" + ordered.OrdersId + "</b><br/>" + "Tên Khách hàng: <b>" + us.Name + "</b><br/>" + "Ngày đặt hàng: <b>" + ordered.DateOrder + "</b><br/>" + "Tổng đơn hàng: <b>" + @String.Format("${0:#,##0.00}", ordered.TotalPrice) + "</b><br/>" + "Sản phẩm đã đặt mua: <br/>" + s + "<br/>" + "Cảm ơn!<br/><br/>";

			//    }


			var smtp = new SmtpClient
			{
				Host = "smtp.gmail.com",
				Port = 587,
				EnableSsl = true,
				DeliveryMethod = SmtpDeliveryMethod.Network,
				UseDefaultCredentials = false,
				Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
			};

			using (var message = new MailMessage(fromEmail, toEmail)
			{
				Subject = subject,
				Body = body,
				IsBodyHtml = true
			})
				smtp.Send(message);

		}


	}
}

