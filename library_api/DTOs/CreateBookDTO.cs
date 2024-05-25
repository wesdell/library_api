using library_api.Validations;
using System.ComponentModel.DataAnnotations;

namespace library_api.DTOs
{
	public class CreateBookDTO
	{
		[Required]
		[StringLength(maximumLength: 250)]
		[FirstLetterCapitalized]
		public string Title { get; set; }
		public DateTime ReleaseDate { get; set; }
		public List<int> AuthorsIds { get; set; }
	}
}