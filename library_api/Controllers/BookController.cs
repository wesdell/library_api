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
			if (createBookDTO.AuthorsIds == null)
			{
				return BadRequest("Cannot create a book without at least one author.");
			}

			List<int> authorsIds = await this._context.Author.Where(author => createBookDTO.AuthorsIds.Contains(author.Id)).Select(author => author.Id).ToListAsync();
			if (createBookDTO.AuthorsIds.Count != authorsIds.Count)
			{
				return BadRequest("There is no one of the submitted authors.");
			}

			Book book = this._mapper.Map<Book>(createBookDTO);

			if (book.AuthorBooks != null)
			{
				for (int i = 0; i < book.AuthorBooks.Count; i += 1)
				{
					book.AuthorBooks[i].Order = i;
				}
			}

			this._context.Add(book);
			await this._context.SaveChangesAsync();
			return Ok();
		}
	}
}