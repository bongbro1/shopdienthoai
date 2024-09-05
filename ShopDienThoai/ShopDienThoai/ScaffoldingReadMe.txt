Scaffolding has generated all the files and added the required dependencies.

However the Application's Startup code may require additional changes for things to work end to end.
Add the following code to the Configure method in your Application's Startup class if not already done:

        app.UseEndpoints(endpoints =>
        {
          endpoints.MapControllerRoute(
            name : "areas",
            pattern : "{area:exists}/{controller=Home}/{action=Index}/{id?}"
          );
        });
@product.Price.ToString("C", CultureInfo.GetCultureInfo("en-US"))

@String.Format("{0:N0} đ", product.Price)


    @String.Format("${0:#,##0.00}", total)

    TempData.Put("Order", order); --> var cartList = TempData.Get<List<Areas.Admin.Models.customerOrder>>("CartList");

    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

  ---
    // Chuyển đổi đối tượng Order thành chuỗi JSON
    var orderJson = JsonConvert.SerializeObject(order);
    // Lưu trữ chuỗi JSON vào Session
    HttpContext.Session.SetString("Order", orderJson);

    var orderJson = HttpContext.Session.GetString("Order");
    // Chuyển đổi chuỗi JSON thành đối tượng Order
    var order = JsonConvert.DeserializeObject<Order>(orderJson);
  ---
    builder.Services.AddControllersWithViews()
    builder.Services.AddSession();
    app.UseSession();
  ---
    var listCart = JsonConvert.SerializeObject(cartItems);  =>> var cartList = JsonConvert.DeserializeObject<List<Areas.Admin.Models.customerOrder>>(order.cartList); // convert nó sang object dạng string để lưu vào csdl
  ---
    var checkedCategories = [];
			$('.js_category, #select_sort, #price-min, #price-max').change(async function () {
				// Lấy giá trị của category và sortBy
				var categoryValue = $('.js_category:checked').map(function () {
					return this.value;
				}).get();
				var sortBy = $('#select_sort').val();
				var priceMin = $('#price-min').val();
				var priceMax = $('#price-max').val();


				try {
					// Gửi yêu cầu fetch đến endpoint '/Products/Fillter/' với phương thức POST
					const response = await fetch('/Products/Fillter/', {
						method: 'POST',
						headers: {
							'Content-Type': 'application/json',
							'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
						},
						// Chuyển đổi dữ liệu thành chuỗi JSON và gửi đi
						body: JSON.stringify({ select_sort: sortBy, selected_categories: categoryValue, price_Min: priceMin, price_Max: priceMax })
					});

					// Nhận dữ liệu phản hồi từ server dưới dạng văn bản
					const data = await response.text();

					// Cập nhật danh sách sản phẩm trên giao diện với dữ liệu nhận được
					$('.store-wrapper').html(data);
				} catch (error) {
					// Ghi log lỗi nếu có
					console.error(error);
				}
			});

	---
		int userId = int.Parse(User.FindFirstValue("Id"));
	--
*Sử dụng trong view
		để sử dụng hàm thông thường thì ở lớp controller tạo class IProductService interface có các phương thức muốn sử dụng. kế thừa lớp abstract IProductService và tạo hàm, sau đó 
		sau đó vào view muốn sử dung thì khai báo
			khái báo trong program.cs : builder.Services.AddScoped<IProductService, ProductsController>();
				@inject WebDienThoai.Controllers.IProductService ProductService
		và gọi phương thức bằng cách IProductService.TenPhuongThuc

	---
	Khi click vào 1 link thì thực hiện chức năng và vẫn ở link trước đó, phục vụ cho chức năng như thêm, xóa sản phẩm khỏi giỏ hàng:
		string referer = Request.Headers["Referer"].ToString();

			// Chuyển hướng đến trang trước đó
			return Redirect(referer);
--
	Sử dụng notification: https://codewithmukesh.com/blog/toast-notifications-in-aspnet-core/
