using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Laboris.Models
{
    public class Products
    {
        
        public int Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public double? OldPrice { get; set; }

        public double? Percent { get; set; }

        public string? PhotoUrl { get; set; }

        public string? Description { get; set; }

        public string? Color { get; set; }

        public string? Size { get; set; }

        [NotMapped]
        public IFormFile ImgFile { get; set; }


        public ICollection<ProductTag>? ProductTags { get; set; } 

        public ProductsCategory? Category { get; set; }

        public List<ProductImages>? ProductImages { get; set; }

        public int? CategoryId { get; set; }

    }
}
