using System;
using System.ComponentModel.DataAnnotations;

namespace Laboris.ViewModels
{
	public class LoginVm
	{

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]

        public string Password { get; set; }
        public bool RememberMe { get; set; }

      
    }
}

