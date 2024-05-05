using library_api.Interfaces;
using library_api.Validations;

namespace library_api.Entities
{
	public class Book : IBook
	{
		public int Id { get; set; }
		[FirstLetterCapitalized]
		public string Title { get; set; }
	}
}