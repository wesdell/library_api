using library_api.Validations;
using System.ComponentModel.DataAnnotations;

namespace library_api.DTOs
{
	public class CreateBookDTO
	{
		[StringLength(maximumLength: 250)]
		[FirstLetterCapitalized]
		public string Title { get; set; }
	}
}