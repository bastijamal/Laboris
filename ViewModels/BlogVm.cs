using System;
using Laboris.Models;

namespace Laboris.ViewModels
{
	public class BlogVm
	{
        public List<Blog> Blogs { get; set; }

        public List<BlogCategory> Categories { get; set; }

        public int? CategoryId { get; set; }
        public string Search { get; set; }
    }
}

