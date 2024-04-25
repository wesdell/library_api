using library_api.Entities;
using Microsoft.AspNetCore.Mvc;

namespace library_api.Controllers
{
	[ApiController]
	[Route("api/authors")]
	public class AuthorContoller : ControllerBase
	{
		[HttpGet]
		public ActionResult<List<Author>> Get()
		{
			return Ok(new List<Author>() { new Author() { Id = 0, Name = "Jane Doe" } });
		}

	}
}