﻿using System;
using Microsoft.AspNetCore.Identity;

namespace Laboris.Models;

public class User:IdentityUser
{
	public string FirstName { get; set; }
	public string LastName { get; set; }


    //

    public List<Order> Orders { get; set; }
}
