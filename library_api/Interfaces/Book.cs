﻿namespace library_api.Interfaces
{
	public interface IBook
	{
		int Id { get; set; }
		int AuthorId { get; set; }
		string Title { get; set; }
		IAuthor Author { get; set; }
	}
}
