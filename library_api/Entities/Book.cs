using library_api.Interfaces;

namespace library_api.Entities
{
	public class Book : IBook
	{
		public int Id { get; set; }
		public int AuthorId { get; set; }
		public string Title { get; set; }
		public IAuthor Author { get; set; }
	}
}
