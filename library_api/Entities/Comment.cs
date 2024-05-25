using library_api.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace library_api.Entities
{
	public class Comment : IComment
	{
		public int Id { get; set; }
		[Required]
		public string Content { get; set; }
		public int BookId { get; set; }
		public Book Book { get; set; }
	}
}