using library_api.Interfaces;
using library_api.Validations;
using System.ComponentModel.DataAnnotations;

namespace library_api.Entities
{
	public class Book : IBook
	{
		public int Id { get; set; }
		[Required]
		[StringLength(maximumLength: 250)]
		[FirstLetterCapitalized]
		public string Title { get; set; }
		public List<Comment> Comments { get; set; }
	}
}