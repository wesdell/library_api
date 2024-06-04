using library_api.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace library_api.Controllers.V1
{
	[ApiController]
	[Route("api/v1")]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class RootController : ControllerBase
	{
		private readonly IAuthorizationService _authorizationService;

		public RootController(IAuthorizationService authorizationService)
		{
			this._authorizationService = authorizationService;
		}

		[HttpGet(Name = "GetRootV1")]
		[AllowAnonymous]
		public async Task<ActionResult<IEnumerable<HATEOASData>>> Get()
		{
			AuthorizationResult isAdmin = await this._authorizationService.AuthorizeAsync(User, "Admin");

			List<HATEOASData> hateoasData = new List<HATEOASData>() {
				new HATEOASData(link: Url.Link("GetRoot", new {}), description: "self", method: "GET"),
				new HATEOASData(link: Url.Link("GetAuthors", new {}), description: "authors", method: "GET")
			};

			if (isAdmin.Succeeded)
			{
				hateoasData.Add(new HATEOASData(link: "CreateAuthor", description: "create-author", method: "POST"));
			}

			return hateoasData;
		}
	}
}