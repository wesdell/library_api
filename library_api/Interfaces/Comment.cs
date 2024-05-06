using library_api.Entities;

namespace library_api.Interfaces
{
	public interface IComment
	{
		int Id { get; set; }
		string Content { get; set; }
		int BookId { get; set; }
		Book Book { get; set; }
	}
}