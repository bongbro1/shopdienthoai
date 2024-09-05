using Microsoft.AspNetCore.Mvc;
using ShopDienThoai.Data;
using ShopDienThoai.Models;
using System.Security.Claims;
using System.Security.Cryptography;

namespace ShopDienThoai.Controllers
{
	public class ShoppingController : Controller
	{
		private readonly ShopDienThoaiContext _context;


		public ShoppingController(ShopDienThoaiContext context)
		{
			_context = context;
		}
		public IActionResult GetProductIdInWishList()
		{
			var productidlist = new List<int>();
			if (User.Identity.IsAuthenticated)
			{
				int userid = int.Parse(User.FindFirstValue("id"));
				productidlist = _context.WishList.Where(x => x.UserId == userid).Select(x => x.ProductId).ToList();
			}
			

			return Json(new { productidlist = productidlist });
		}
		public IActionResult GetWishListId ()
		{
			var wishlistid = _context.WishList.Select(x => x.WishlistId).ToList();


			return Json(new { wishlistid = wishlistid });
		}
		public IActionResult AddToWishList (int productid)
		{
			if (User.Identity.IsAuthenticated)
			{
				int userid = int.Parse(User.FindFirstValue("id"));
				var old = _context.WishList.Where(x => x.UserId == userid && x.ProductId == productid).FirstOrDefault();
				if (old == null)
				{
					WishList obj = new WishList()
					{
						ProductId = productid,
						UserId = userid,
						AddedDate = DateTime.Now
					};
					_context.Add(obj);
					_context.SaveChanges();
					TempData["ModalType"] = "success";
					TempData["ModalTitle"] = "Thành công";
					TempData["ModalMessage"] = "Đã thêm sản phẩm vào WishList!";
				} else
				{
					_context.Remove(old);
					_context.SaveChanges();
					TempData["ModalType"] = "success";
					TempData["ModalTitle"] = "Thành công";
					TempData["ModalMessage"] = "Đã xóa sản phẩm khỏi WishList!";
				}
				
				
			}
			else
			{
				TempData["ModalType"] = "error";
				TempData["ModalTitle"] = "Thất bại";
				TempData["ModalMessage"] = "Vui lòng đăng nhập!";
			}
			string referer = Request.Headers["Referer"].ToString();
			return Redirect(referer);

		}

		public IActionResult RemoveFromWishList (int id)
		{
			var old = _context.WishList.Where(x => x.WishlistId == id).First();
			if (old != null)
			{
				_context.Remove(old);
				_context.SaveChanges();
				TempData["ModalType"] = "success";
				TempData["ModalTitle"] = "Thành công";
				TempData["ModalMessage"] = "Đã xóa sản phẩm khỏi WishList!";
			}
			string referer = Request.Headers["Referer"].ToString();
			return Redirect(referer);
		}

		public IActionResult RemoveAllFromWishList()
		{
			if (User.Identity.IsAuthenticated)
			{
				int userid = int.Parse(User.FindFirstValue("id"));
				var olds = _context.WishList.Where(x => x.UserId == userid).ToList();
				if (olds.Count != 0)
				{
					foreach (var old in olds)
					{
						_context.Remove(old);
					}

					_context.SaveChanges();
					TempData["ModalType"] = "success";
					TempData["ModalTitle"] = "Thành công";
					TempData["ModalMessage"] = "Đã xóa sản phẩm khỏi WishList!";
				}
			}
			
			string referer = Request.Headers["Referer"].ToString();
			return Redirect(referer);
		}
	}
}
