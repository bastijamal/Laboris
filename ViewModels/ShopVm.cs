using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

using Laboris.Models;

namespace Laboris.ViewModels
{
	public class ShopVm
	{

		public int? Order { get; set; }

        public int? CategoryId { get; set; }

        public string? Search { get; set; }

        //
        public string? SelectedColor { get; set; }
        //

        public List<Products> Products { get; set; }

		public List<ProductsCategory> Categories { get; set; }

        public List<Tag> Tags { get; set; } 
    }
}

