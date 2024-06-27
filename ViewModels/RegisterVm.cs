using System;
using System.ComponentModel.DataAnnotations;

namespace Laboris.ViewModels
{
	public class RegisterVm
	{
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }


        [DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}

