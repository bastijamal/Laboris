using System;
using System.ComponentModel.DataAnnotations;
using Laboris.Models;

namespace Laboris.ViewModels
{
	public class OrderVm
	{
        [Required]



		public string? AdressDistrc { get; set; }
        public string? AdressStreet { get; set; }


        [DataType(DataType.PostalCode)]
        public string? Postcode { get; set; }


        [DataType(DataType.PhoneNumber)]
        public string? Phone { get; set; }

        public string LastName { get; set; }
        public string FirstName { get; set; }

        public string? Notes { get; set; }

        [EmailAddress]
        public string EmailAdress { get; set; }

        public List<LoginVm> loginVms { get; set; }
	}
}

