using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShopDienThoai.Controllers;
using ShopDienThoai.Data;
using ShopDienThoai.Models.Other_Model;
namespace ShopDienThoai
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<ShopDienThoaiContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("ShopDienThoaiContext") ?? throw new InvalidOperationException("Connection string 'ShopDienThoaiContext' not found.")));

            // Add services to the container.
            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                options.SlidingExpiration = true;
                options.AccessDeniedPath = "/Forbidden/";
            });

			builder.Services.AddTransient<IViewRenderingService, ViewRenderingService>();
			builder.Services.AddHttpContextAccessor();


			builder.Services.AddScoped<IEmailService, EmailService>();
			builder.Services.AddScoped<IProductService, ProductsController>();

			var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


			app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "admin_default",
                    pattern: "Admin/{controller=Home}/{action=Index}/{id?}",
                    defaults: new { area = "Admin" });
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.Run();
        }
    }
}
