namespace library_api.DTOs
{
	public class AuthorDTOBooks : AuthorDTO
	{
		public List<BookDTO> Books { get; set; }
	}
}