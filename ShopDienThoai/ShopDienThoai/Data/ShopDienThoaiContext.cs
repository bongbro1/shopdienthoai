using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopDienThoai.Models;

namespace ShopDienThoai.Data
{
    public class ShopDienThoaiContext : DbContext
    {
        public ShopDienThoaiContext (DbContextOptions<ShopDienThoaiContext> options)
            : base(options)
        {
        }

        public DbSet<ShopDienThoai.Models.Cart> Cart { get; set; } = default!;
        public DbSet<ShopDienThoai.Models.Category> Category { get; set; } = default!;
        public DbSet<ShopDienThoai.Models.Order> Order { get; set; } = default!;
        public DbSet<ShopDienThoai.Models.OrderDetail> OrderDetail { get; set; } = default!;
        public DbSet<ShopDienThoai.Models.Payment> Payment { get; set; } = default!;
        public DbSet<ShopDienThoai.Models.Product> Product { get; set; } = default!;
        public DbSet<ShopDienThoai.Models.Review> Review { get; set; } = default!;
        public DbSet<ShopDienThoai.Models.User> User { get; set; } = default!;
        public DbSet<ShopDienThoai.Models.WishList> WishList { get; set; } = default!;
    }
}
