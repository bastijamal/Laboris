using System;
namespace Laboris.Models
{
	public class ProductImages
	{
        public int Id { get; set; }

        public string? ImgUrl { get; set; }


        public bool IsPrimary { get; set; } = false;

        public int ProductId { get; set; }

        public Products Product { get; set; }
    }
}

