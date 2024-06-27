using System;
using Laboris.Models;
using Microsoft.EntityFrameworkCore;

namespace Laboris.ViewModels
{
	public class HomeVm
	{
		public List<Products> Products { get;set;}
        public List<Blog> Blogs { get; set; }
        public List<Customers> Customers { get; set; }
    }

}

