using Microsoft.AspNetCore.Mvc;

namespace ShopDienThoai.Models.Components_Model
{
	public class _header : ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View();
		}
	}
}
