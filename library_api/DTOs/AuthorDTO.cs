using System.ComponentModel.DataAnnotations;

namespace library_api.DTOs
{
	public class AuthorDTO
	{
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
	}
}