﻿using library_api.Interfaces;
using library_api.Validations;
using System.ComponentModel.DataAnnotations;

namespace library_api.Entities
{
	public class Book : IBook
	{
		public int Id { get; set; }
		[Required]
		[StringLength(maximumLength: 250)]
		[FirstLetterUppercaseAttribute]
		public string Title { get; set; }
		public DateTime? ReleaseDate { get; set; }
		public List<Comment> Comments { get; set; }
		public List<AuthorBook> AuthorBooks { get; set; }
	}
}