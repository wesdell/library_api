using library_api.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace library_api.Controllers
{
	[ApiController]
	[Route("api")]
	public class RootController : ControllerBase
	{
		[HttpGet(Name = "GetRoot")]
		public ActionResult<IEnumerable<HATEOASData>> Get()
		{
			List<HATEOASData> hateoasData = new List<HATEOASData>();
			hateoasData.Add(new HATEOASData(link: Url.Link("GetRoot", new { }), description: "self", method: "GET"));
			return hateoasData;
		}
	}
}