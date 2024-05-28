﻿using System.ComponentModel.DataAnnotations;

namespace library_api.DTOs
{
	public class UserCredentials
	{
		[Required]
		public string Name { get; set; }
		[Required]
		[EmailAddress]
		public string Email { get; set; }
		[Required]
		public string Password { get; set; }
	}
}