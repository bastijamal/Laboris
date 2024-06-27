using System;
namespace Laboris.Models
{
	public class ProductTag
	{
		public int Id { get; set; }
		public Products Product { get; set; }
		public int ProductId { get; set; }
		public Tag Tag { get; set; }
		public int TagId { get; set; }
	}
}

