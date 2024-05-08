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
			CreateMap<Author, AuthorDTO>().ForMember(author => author.Books, options => options.MapFrom(this.MapAuthorDTOBook));
			CreateMap<CreateBookDTO, Book>().ForMember(book => book.AuthorBooks, options => options.MapFrom(this.MapAuthorBook));
			CreateMap<Book, BookDTO>().ForMember(book => book.Authors, options => options.MapFrom(this.MapBookDTOAuthor));
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

		private List<AuthorDTO> MapBookDTOAuthor(Book book, BookDTO bookDTO)
		{
			List<AuthorDTO> authorDTOs = new List<AuthorDTO>();
			if (book.AuthorBooks == null)
			{
				return authorDTOs;
			}
			foreach (var authorbook in book.AuthorBooks)
			{
				authorDTOs.Add(new AuthorDTO() { Id = authorbook.AuthorId, Name = authorbook.Author.Name });
			}
			return authorDTOs;
		}

		private List<BookDTO> MapAuthorDTOBook(Author author, AuthorDTO authorDTO)
		{
			List<BookDTO> bookDTOs = new List<BookDTO>();
			if (author.AuthorBooks == null)
			{
				return bookDTOs;
			}
			foreach (var authorbook in author.AuthorBooks)
			{
				bookDTOs.Add(new BookDTO() { Id = authorbook.BookId, Title = authorbook.Book.Title });
			}
			return bookDTOs;
		}
	}
}