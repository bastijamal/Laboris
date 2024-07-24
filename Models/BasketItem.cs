using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Laboris.Models
{
	public class BasketItem
	{
		public int Id { get; set; }

		public string UserId { get; set; } = null!;

		public User User { get; set; } = null!;

		public Products Product { get; set; } = null!;

		public int ProductId { get; set; }


        public int Count { get; set; } = 1;

		public int Price { get; set; }

		//

		public Order? Order { get; set; }

		public int? OrderId { get; set; }



    }
}

