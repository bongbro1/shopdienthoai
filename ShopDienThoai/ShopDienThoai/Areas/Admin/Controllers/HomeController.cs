using Microsoft.AspNetCore.Mvc;
using ShopDienThoai.Data;

namespace ShopDienThoai.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly ShopDienThoaiContext _context;

        public HomeController(ShopDienThoaiContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            return Redirect("/admin/users/login");
            
        }
    }
}
