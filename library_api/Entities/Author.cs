using library_api.Interfaces;

namespace library_api.Entities
{
	public class Author : IAuthor
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public List<IBook> Books { get; set; }
	}
}