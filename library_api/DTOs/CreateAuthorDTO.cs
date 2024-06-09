using library_api.Validations;
using System.ComponentModel.DataAnnotations;

namespace library_api.DTOs
{
	public class CreateAuthorDTO
	{
		[Required]
		[StringLength(maximumLength: 150)]
		[FirstLetterUppercaseAttribute]
		public string Name { get; set; }
	}
}