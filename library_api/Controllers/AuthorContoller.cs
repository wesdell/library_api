using library_api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace library_api.Controllers
{
	[ApiController]
	[Route("api/authors")]
	public class AuthorContoller : ControllerBase
	{
		private readonly ApplicationDBContext _context;

		public AuthorContoller(ApplicationDBContext context)
		{
			this._context = context;
		}

		[HttpGet]
		public async Task<ActionResult<List<Author>>> Get()
		{
			return await this._context.Author.ToListAsync();
		}

		[HttpPost]
		public async Task<ActionResult> Post(Author author)
		{
			this._context.Add(author);
			await this._context.SaveChangesAsync();
			return Ok();
		}

		[HttpPut("{id:int}")]
		public async Task<ActionResult> Put(Author author, int id)
		{
			if (author.Id != id)
			{
				return BadRequest("Author id does not match with any record.");
			}

			bool authorExists = await this._context.Author.AnyAsync(author => author.Id == id);
			if (!authorExists)
			{
				return NotFound();
			}

			this._context.Update(author);
			await this._context.SaveChangesAsync();
			return Ok();
		}

		[HttpDelete("{id:int}")]
		public async Task<ActionResult> Delete(int id)
		{
			bool authorExists = await this._context.Author.AnyAsync(author => author.Id == id);
			if (!authorExists)
			{
				return NotFound();
			}

			this._context.Remove(new Author() { Id = id });
			await this._context.SaveChangesAsync();
			return Ok();
		}
	}
}