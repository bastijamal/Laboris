using System;
namespace Laboris.Models
{
	public class ProductTag
	{
        public int Id { get; set; }

        public string Name { get; set; }

        public Products Product { get; set; }
        public int ProductId { get; set; }

        public Tag ?Tag { get; set; }
        public int TagId { get; set; }

        //public ICollection<Tag>? Tags { get; set; }


    }
}

