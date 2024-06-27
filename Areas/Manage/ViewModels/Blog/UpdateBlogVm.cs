using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Laboris.Areas.Manage.ViewModels.Blog
{
	public class UpdateBlogVm
	{

        public int? Id { get; set; }

        public string Header { get; set; }

        [MinLength(20)]
        [MaxLength(50)]
        public string Description { get; set; }

        public string Admin { get; set; }

        [DataType(DataType.Date)]
        public string Date { get; set; }


        public int? CategoryId { get; set; }


        public string? PhotoUrl { get; set; }

        [NotMapped]
        public IFormFile ImgFile { get; set; }


    }
}

