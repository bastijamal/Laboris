 using System;
using System.ComponentModel.DataAnnotations;

namespace Laboris.Models
{
	public class Order
	{
		
		public int Id { get; set; }

		public bool? Status { get; set; }

		public double TotalPrice { get; set; }

        public DateTime PurchaseAt { get; set; }

		public string UserId { get; set; } = null!;

		public User User { get; set; } = null!;

		public string Adress { get; set; }

		public string Postcode { get; set; }


		public List<BasketItem> basketItems { get; set; } = new();
        public List<WishlistItem> wishlistItems { get; set; } = new();
    }
}

