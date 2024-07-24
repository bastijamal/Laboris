using System;
using System.ComponentModel.DataAnnotations;

namespace Laboris.Models
{
	public class ProductsCategory
	{
        public int Id { get; set; }

        [MinLength(3)]
        public string Name { get; set; }

        public ICollection<Products>? Products { get; set; }

    }
}

