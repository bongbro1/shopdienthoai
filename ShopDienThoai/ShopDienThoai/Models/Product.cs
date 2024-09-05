﻿using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace ShopDienThoai.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public string? Brand { get; set; }
        public int? StockQuantity { get; set; }
        public string? ImageURL { get; set; }

        // Navigation property
        public Category? Category { get; set; }
    }
}
