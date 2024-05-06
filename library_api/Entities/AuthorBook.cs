using library_api.Interfaces;

namespace library_api.Entities
{
	public class AuthorBook : IAuthorBook
	{
		public int AuthorId { get; set; }
		public int BookId { get; set; }
		public int Order { get; set; }
		public Author Author { get; set; }
		public Book Book { get; set; }
	}
}