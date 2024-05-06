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
			CreateMap<CreateBookDTO, Book>();
			CreateMap<Book, BookDTO>();
		}
	}
}