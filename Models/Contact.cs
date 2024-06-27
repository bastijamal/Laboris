using System;
using System.ComponentModel.DataAnnotations;

namespace Laboris.Models
{
	public class Contact
	{
		public int Id { get; set; }

		public string Name { get; set; }

		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		public string Subject { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

		public string Comment { get; set; }
	}
}

