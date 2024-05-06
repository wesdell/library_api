using AutoMapper;
using library_api.DTOs;
using library_api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace library_api.Controllers
{
	[ApiController]
	[Route("api/book")]
	public class BookController : ControllerBase
	{
		private readonly ApplicationDBContext _context;
		private readonly IMapper _mapper;

		public BookController(ApplicationDBContext context, IMapper mapper)
		{
			this._context = context;
			this._mapper = mapper;
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<BookDTO>> GetById(int id)
		{
			bool bookExists = await this._context.Book.AnyAsync(book => book.Id == id);
			if (!bookExists)
			{
				return NotFound();
			}
			Book book = await this._context.Book.FirstOrDefaultAsync(book => book.Id == id);
			return this._mapper.Map<BookDTO>(book);
		}

		[HttpPost]
		public async Task<ActionResult> Post([FromBody] CreateBookDTO createBookDTO)
		{
			//bool authorExists = await this._context.Author.AnyAsync(author => author.Id == book.AuthorId);
			//if (!authorExists)
			//{
			//	return BadRequest($"Author: {book.AuthorId}, does not exist.");
			//}
			Book book = this._mapper.Map<Book>(createBookDTO);
			this._context.Add(book);
			await this._context.SaveChangesAsync();
			return Ok();
		}
	}
}