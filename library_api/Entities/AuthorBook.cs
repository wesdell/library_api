﻿namespace library_api.Entities
{
	public class AuthorBook
	{
		public int AuthorId { get; set; }
		public int BookId { get; set; }
		public int Order { get; set; }
		public Author Author { get; set; }
		public Book Book { get; set; }
	}
}