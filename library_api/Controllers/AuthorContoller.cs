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
	}
}