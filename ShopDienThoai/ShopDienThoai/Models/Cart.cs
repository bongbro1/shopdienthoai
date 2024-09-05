﻿namespace ShopDienThoai.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        // Navigation properties
        public User? User { get; set; }
        public Product? Product { get; set; }
    }
}
