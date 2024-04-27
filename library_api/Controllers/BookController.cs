using library_api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace library_api.Controllers
{
	[ApiController]
	[Route("api/books")]
	public class BookController : ControllerBase
	{
		private readonly ApplicationDBContext _context;

		public BookController(ApplicationDBContext context)
		{
			this._context = context;
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<Book>> GetById(int id)
		{
			bool bookExists = await this._context.Book.AnyAsync(book => book.Id == id);
			if (!bookExists)
			{
				return NotFound();
			}

			return await this._context.Book.Include(book => book.Author).FirstOrDefaultAsync(book => book.Id == id);
		}

		[HttpPost]
		public async Task<ActionResult> Post(Book book)
		{
			bool authorExists = await this._context.Author.AnyAsync(author => author.Id == book.AuthorId);
			if (!authorExists)
			{
				return BadRequest($"Author: {book.AuthorId}, does not exist.");
			}

			this._context.Add(book);
			await this._context.SaveChangesAsync();
			return Ok();
		}
	}
}
