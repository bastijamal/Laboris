﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Laboris.Areas.Manage.ViewModels.Product
{
	public class ProductUpdateVm
	{
        public int Id { get; set; }

      
  
        public string Name { get; set; }

        public double Price { get; set; }

        public double? OldPrice { get; set; }

        public double? Percent { get; set; }


        public IFormFile ImgFile { get; set; }

        public List<int> TagIds { get; set; } = new List<int>();



        public int? CategoryId { get; set; }


        public string? Description { get; set; }

        public string? Color { get; set; }

        public string? Size { get; set; }
        
    }
}

