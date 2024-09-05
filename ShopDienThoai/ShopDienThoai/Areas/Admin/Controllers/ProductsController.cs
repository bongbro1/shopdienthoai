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

namespace ShopDienThoai.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly ShopDienThoaiContext _context;
        private readonly ICompositeViewEngine _viewEngine;

        private readonly string _imageFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "asset" ,"img");
        private List<string> brands = new List<string>
        {
            "Laptop nổi bật",
            "Phụ kiện nổi bật",
            "Đồng hồ thông minh",
            "Máy tính bảng",
            "Điện thoại nổi bật nhất",
            "Sản phẩm mới",
            "Hotsale giá tốt"
        };

        public ProductsController(ShopDienThoaiContext context, ICompositeViewEngine viewEngine)
        {
            _context = context;
            _viewEngine = viewEngine;
        }

        // GET: Admin/Products
        public async Task<IActionResult> Index()
        {
            var shopDienThoaiContext = _context.Product.Include(p => p.Category);
            return View(await shopDienThoaiContext.ToListAsync());
        }

        // GET: Admin/Products/Details/5
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

            return View(product);
        }

        // GET: Admin/Products/Create
        public IActionResult Create()
        {
            // Lấy danh sách danh mục và thương hiệu để hiển thị trong dropdown
            ViewBag.Categories = _context.Category.ToList();
            ViewBag.Brands = brands;

            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, List<IFormFile> ImageFiles)
        {
            if (ModelState.IsValid)
            {
                var imageUrls = new List<string>();
                // Xử lý hình ảnh
                if (ImageFiles != null && ImageFiles.Count > 0)
                {
                    foreach (var file in ImageFiles)
                    {
                        if (file.Length > 0)
                        {
                            // Tạo đường dẫn để lưu hình ảnh
                            var fileName = Path.GetFileName(file.FileName);
                            var filePath = Path.Combine(_imageFolderPath, fileName);

                            // Lưu hình ảnh vào thư mục
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            // Tạo URL hình ảnh
                            // Lưu sản phẩm vào cơ sở dữ liệu
                            imageUrls.Add(fileName);
                        }
                    }
                }
                // Cập nhật thuộc tính ImageURL với danh sách URL hình ảnh (hoặc bạn có thể chỉ lưu URL của hình ảnh đầu tiên nếu chỉ cần một hình ảnh)
                product.ImageURL = string.Join(";", imageUrls);
                _context.Product.Add(product);
                await _context.SaveChangesAsync();
                return Redirect("/admin/products");
            }

            // Nếu có lỗi, lấy danh sách danh mục và thương hiệu và hiển thị lại form
            ViewBag.Categories = _context.Category.ToList();
            ViewBag.Brands = _context.Product.Select(p => p.Brand).Distinct().ToList();
            return View(product);
        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.Categories = _context.Category.ToList();
            ViewBag.Brands = brands;
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ProductId,ProductName,Price,Description,CategoryId,Brand,StockQuantity,ImageURL")] Product product, List<IFormFile> ImageFiles)
        {

            if (ModelState.IsValid)
            {
                try
                {
					var prOld = _context.Product.AsNoTracking().Where(x => x.ProductId == id).FirstOrDefault();
					var imageUrls = new List<string>();
					if (ImageFiles.Count == 0)
					{
						imageUrls = prOld.ImageURL.Split(";").ToList();
					}

					// Xử lý hình ảnh
					if (ImageFiles != null && ImageFiles.Count > 0)
					{
						foreach (var file in ImageFiles)
						{
							if (file.Length > 0)
							{
								// Tạo đường dẫn để lưu hình ảnh
								var fileName = Path.GetFileName(file.FileName);
								var filePath = Path.Combine(_imageFolderPath, fileName);

								// Lưu hình ảnh vào thư mục
								using (var stream = new FileStream(filePath, FileMode.Create))
								{
									file.CopyToAsync(stream);
								}

								// Tạo URL hình ảnh
								imageUrls?.Add(fileName);
							}
						}
					}
					if (prOld != null)
					{
						// Cập nhật thuộc tính của thực thể đã được theo dõi
						product.ImageURL = string.Join(";", imageUrls);
						// Không cần gọi _context.Update(product)
					}
					_context.Update(product);
					_context.SaveChanges();
                    return Redirect("/admin/products");
				} catch (Exception ex) { }

			}
            ViewBag.Categories = _context.Category.ToList();
            ViewBag.Brands = brands;
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
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
            ViewBag.Categories = _context.Category.ToList();
            ViewBag.Brands = brands;
            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }

            await _context.SaveChangesAsync();
            return Redirect("/admin/products");
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }

        public IActionResult LoadMoreProducts(string sourceBrand, int skip, int take)
        {
            // Lấy các sản phẩm mới
            var list = _context.Product.Include(p => p.Category).Where(p => p.Brand == sourceBrand);
            var products = list.Skip(skip).Take(take).ToList();

            // Kiểm tra nếu còn sản phẩm chưa được hiển thị
            var hasMoreProducts = list.Count() > skip + take;

            // Trả về Partial View cùng với thông tin về việc có còn sản phẩm hay không
            return Json(new
            {
                html = RenderPartialViewToString("_ProductListPartial", products),
                hasMore = hasMoreProducts
            });
        }

        // Hàm Render PartialView ra chuỗi (nếu chưa có)
        public string RenderPartialViewToString(string viewName, object model)
        {
            // Gán model cho ViewData
            ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                // Sử dụng ICompositeViewEngine để tìm view
                var viewResult = _viewEngine.FindView(ControllerContext, viewName, false);

                if (!viewResult.Success)
                {
                    throw new InvalidOperationException($"Không thể tìm thấy view có tên {viewName}");
                }

                // Tạo ViewContext
                var viewContext = new ViewContext(
                    ControllerContext,
                    viewResult.View,
                    ViewData,
                    TempData,
                    sw,
                    new HtmlHelperOptions()
                );

                // Render view
                viewResult.View.RenderAsync(viewContext).Wait();

                return sw.GetStringBuilder().ToString();
            }
        }


    }
}
