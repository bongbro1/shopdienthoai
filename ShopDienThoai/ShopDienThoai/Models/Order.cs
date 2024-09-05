namespace ShopDienThoai.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string ShippingAddress { get; set; }
        public string OrderStatus { get; set; } = "Pending";

        // Navigation property
        public User? User { get; set; }
        public Payment? Payment { get; set; }
    }
}
