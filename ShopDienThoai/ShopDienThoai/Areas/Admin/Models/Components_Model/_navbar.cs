using Microsoft.AspNetCore.Mvc;

namespace ShopDienThoai.Areas.Admin.Models.Components_Model
{
    public class _navbar : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
