using System;
using Laboris.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Laboris.Areas.Manage.ViewModels.Blog
{
	public class CreateBlogVm
	{
        public string Header { get; set; }

  
        public string Description { get; set; }

        public string Admin { get; set; }

        [DataType(DataType.Date)]
        public string Date { get; set; }

        public IFormFile ImgFile { get; set; } = null!;
       

        public int? CategoryId { get; set; }


    }


}


