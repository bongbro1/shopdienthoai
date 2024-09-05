using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using ShopDienThoai.Data;
using ShopDienThoai.Models;
using ShopDienThoai.Models.Other_Model;

namespace ShopDienThoai.Controllers
{
    public interface IProductService
    {
        public Product _getProductByProductid(int id);
    }
    public class ProductsController : Controller, IProductService
	{
        private readonly ShopDienThoaiContext _context;
        private readonly ICompositeViewEngine _viewEngine;
		private readonly IServiceProvider _serviceProvider;
		private readonly IViewRenderingService _viewRenderingService;

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

		public ProductsController(ShopDienThoaiContext context, IViewRenderingService viewRenderingService, IServiceProvider serviceProvider)
        {
            _context = context;
			_viewRenderingService = viewRenderingService;
			_serviceProvider = serviceProvider;
		}

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var shopDienThoaiContext = _context.Product.Include(p => p.Category);
            return View(await shopDienThoaiContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            var relatedProducts = _context.Product.Include(x => x.Category).Where(x => x.ProductId != id && x.CategoryId == product.CategoryId).ToList();
            ViewBag.RelatedProducts = relatedProducts;

            return View(product);
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }

		public IActionResult LoadMoreProducts(string sourceBrand,int skip, int take)
		{
			var brandName = brands.FirstOrDefault(x => x.Key == sourceBrand)?.Name;
			var list = _context.Product
                    .Include(p => p.Category)
                    .Where(p => p.Brand == brandName).AsQueryable();
			var products = list.Skip(skip).Take(take).ToList();
			var hasMoreProducts = list.Count() > skip + take;

            var result = new
            {
                html = _viewRenderingService.RenderPartialView(ControllerContext, "_ProductListPartial", products),
				hasMore = hasMoreProducts
			};

			return Json(result);
		}

		public Product _getProductByProductid(int id)
        {
            Product pro = _context.Product.Include(x => x.Category).Where(x => x.ProductId == id).First();
            return pro;
        }




	}
}
