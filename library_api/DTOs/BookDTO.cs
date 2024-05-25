using System.ComponentModel.DataAnnotations;

namespace library_api.DTOs
{
	public class BookDTO
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public DateTime ReleaseDate { get; set; }
	}
}