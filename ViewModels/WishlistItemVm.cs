﻿using System;
namespace Laboris.ViewModels
{
	public class WishlistItemVm
	{
        public int Id { get; set; }

        public string Name { get; set; }

        public string? Color { get; set; }

        public string? Size { get; set; }

        public string? PhotoUrl { get; set; }

        public int Price { get; set; }
    }
}

