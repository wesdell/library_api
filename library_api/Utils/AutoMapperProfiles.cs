using AutoMapper;
using library_api.DTOs;
using library_api.Entities;

namespace library_api.Utils
{
	public class AutoMapperProfiles : Profile
	{
		public AutoMapperProfiles()
		{
			CreateMap<CreateAuthorDTO, Author>();
			CreateMap<Author, AuthorDTO>();
			CreateMap<CreateBookDTO, Book>().ForMember(book => book.AuthorBooks, options => options.MapFrom(this.MapAuthorBook));
			CreateMap<Book, BookDTO>();
			CreateMap<CreateCommentDTO, Comment>();
			CreateMap<Comment, CommentDTO>();
		}

		private List<AuthorBook> MapAuthorBook(CreateBookDTO createBookDTO, Book book)
		{
			List<AuthorBook> authorBooks = new List<AuthorBook>();
			if (createBookDTO.AuthorsIds == null)
			{
				return authorBooks;
			}
			foreach (int id in createBookDTO.AuthorsIds)
			{
				authorBooks.Add(new AuthorBook() { AuthorId = id });
			}
			return authorBooks;
		}
	}
}