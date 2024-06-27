using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Laboris.Models
{
    public class Blog
	{
        
        public int Id { get; set; }

        public string Header { get; set; }

      
        public string Description { get; set; }

        public string Admin { get; set; }

        [DataType(DataType.Date)]
        public string Date { get; set; }

        public string PhotoUrl { get; set; }


        public int? CategoryId { get; set; }

        public List<Blog> Blogs { get; set; }

        public BlogCategory? Category { get; set; }

        public string? Search { get; set; }


    }
}

