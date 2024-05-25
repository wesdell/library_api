﻿using library_api.Entities;
using System.ComponentModel.DataAnnotations;

namespace library_api.DTOs
{
	public class CommentDTO
	{
		public int Id { get; set; }
		[Required]
		public string Content { get; set; }
	}
}