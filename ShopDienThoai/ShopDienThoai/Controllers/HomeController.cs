using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopDienThoai.Data;
using ShopDienThoai.Models;
using System.Diagnostics;

namespace ShopDienThoai.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ShopDienThoaiContext _context;
        private List<Brand> brands = new List<Brand>
        {
            new Brand { Key = "laptopnoibat", Name = "Laptop nổi bật" },
            new Brand { Key = "phukiennoibat", Name = "Phụ kiện nổi bật" },
            new Brand { Key = "donghothongminh", Name = "Đồng hồ thông minh" },
            new Brand { Key = "maytinhbang", Name = "Máy tính bảng" },
            new Brand { Key = "dienthoainoitbatnhat", Name = "Điện thoại nổi bật nhất" },
            new Brand { Key = "sanphammoi", Name = "Sản phẩm mới" },
            new Brand { Key = "hotsalegiatot", Name = "Hotsale giá tốt" }
        };


        public HomeController(ShopDienThoaiContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
			var productsByBrand = new Dictionary<string, List<Product>>();

			foreach (var brand in brands)
			{
				var products = _context.Product
					.Include(p => p.Category)
					.Where(p => p.Brand == brand.Name)
                    .Take(10)
					.ToList();

				// Thêm vào từ điển
				productsByBrand[brand.Key] = products;
			}

			// Gán từ điển vào ViewBag
			ViewBag.ProductsByBrand = productsByBrand;

			return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
