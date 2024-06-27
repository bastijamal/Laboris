using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Laboris.Models;

	public class Customers
	{
		
    public int Id { get; set; }

    public string Name { get; set; }

    [MinLength(5)]
    [MaxLength(100)]
    public string Comment { get; set; }

  
    public string Position { get; set; }


    public string? PhotoUrl { get; set; }

    [NotMapped]
    public IFormFile ImgFile { get; set; }
}

