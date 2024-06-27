using System;
using Laboris.Models;

namespace Laboris.ViewModels
{
	public class ShopVm
	{

		public int? Order { get; set; }

        public int? CategoryId { get; set; }

        public string? Search { get; set; }

        public List<Products> Products { get; set; }

		public List<ProductsCategory> Categories { get; set; }
	}
}

