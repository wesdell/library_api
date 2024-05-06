using library_api.Interfaces;

namespace library_api.Entities
{
	public class Comment : IComment
	{
		public int Id { get; set; }
		public string Content { get; set; }
		public int BookId { get; set; }
		public Book Book { get; set; }
	}
}