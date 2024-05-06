using library_api.Entities;

namespace library_api.Interfaces
{
	public interface IAuthor
	{
		int Id { get; set; }
		string Name { get; set; }
		List<AuthorBook> AuthorBooks { get; set; }
	}
}