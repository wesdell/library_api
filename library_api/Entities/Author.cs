using library_api.Interfaces;
using library_api.Validations;
using System.ComponentModel.DataAnnotations;

namespace library_api.Entities
{
	public class Author : IAuthor
	{
		public int Id { get; set; }
		[Required]
		[StringLength(maximumLength: 150)]
		[FirstLetterUppercaseAttribute]
		public string Name { get; set; }
		public List<AuthorBook> AuthorBooks { get; set; }
	}
}