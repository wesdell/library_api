using library_api.Entities;

namespace library_api.Interfaces
{
	public interface IBook
	{
		int Id { get; set; }
		string Title { get; set; }
		List<Comment> Comments { get; set; }
	}
}