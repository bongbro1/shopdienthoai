using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShopDienThoai.Models
{
	public class WishList
	{
		[Key]
		public int WishlistId { get; set; }

		[Required]
		public int UserId { get; set; }

		[Required]
		public int ProductId { get; set; }

		[Required]
		public DateTime AddedDate { get; set; } = DateTime.Now;

		public User? User { get; set; }
		public Product? Product { get; set; }
	}
}
