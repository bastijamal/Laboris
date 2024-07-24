using System;
namespace Laboris.ViewModels
{
	public class WishlistCookieItem
	{
        public int Id { get; set; }

        public int Count { get; set; }

        public string? Size { get; set; } = null!;

        public string? Color { get; set; } = null!;
    }
}

