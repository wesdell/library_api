using library_api.Entities;

namespace library_api.Interfaces
{
	public interface IAuthorBook
	{
		int AuthorId { get; set; }
		int BookId { get; set; }
		int Order { get; set; }
		Author Author { get; set; }
		Book Book { get; set; }
	}
}