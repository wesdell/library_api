using System.ComponentModel.DataAnnotations;

namespace library_api.DTOs
{
	public class BookDTO
	{
		public int Id { get; set; }
		[Required]
		public string Title { get; set; }
	}
}