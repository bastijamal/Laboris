using System;
namespace Laboris.Models
{
	public class WishlistItem
	{
        public int Id { get; set; }


        public string UserId { get; set; } = null!;

        public User User { get; set; } = null!;


        public Products Product { get; set; } = null!;

        public int ProductId { get; set; }


        public int Price { get; set; }

        //

        public Order? Order { get; set; }

        public int? OrderId { get; set; }

    }
}

