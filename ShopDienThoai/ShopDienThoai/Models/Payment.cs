namespace ShopDienThoai.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int OrderId { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.Now;
        public string PaymentStatus { get; set; } = "Pending";

        // Navigation property
        public Order? Order { get; set; }
    }
}
