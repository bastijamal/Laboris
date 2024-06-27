using System;
using System.ComponentModel.DataAnnotations;

namespace Laboris.Models
{
	public class BlogCategory
	{
		public int Id { get; set; }

		[MinLength(3)]
		public string Name { get; set; }


        //elave eledm chatgpt

        //public ICollection<Blog> Blogs { get; set; }
    }
}

