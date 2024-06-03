using library_api.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using System;

namespace library_api.Services
{
	public class SetHATEOASLinks
	{
		private readonly IAuthorizationService _authorizationService;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IActionContextAccessor _actionContextAccessor;

		public SetHATEOASLinks(IAuthorizationService authorizationService, IHttpContextAccessor httpContextAccessor, IActionContextAccessor actionContextAccessor)
		{
			this._authorizationService = authorizationService;
			this._httpContextAccessor = httpContextAccessor;
			this._actionContextAccessor = actionContextAccessor;
		}

		public async Task Set(AuthorDTO authorDTO)
		{
			IUrlHelper url = this.BuildUrlHelper();
			bool isAdmin = await this.isAdmin();

			authorDTO.Links.Add(
				new HATEOASData(link: url.Link("GetAuthorById", new { id = authorDTO.Id }), description: "self", method: "GET")
			);

			if (isAdmin)
			{
				authorDTO.Links.Add(
					new HATEOASData(link: url.Link("UpdateAuthorById", new { id = authorDTO.Id }), description: "author-update", method: "PUT")
				);
				authorDTO.Links.Add(
					new HATEOASData(link: url.Link("DeleteAuthorById", new { id = authorDTO.Id }), description: "author-delete", method: "DELETE")
				);
			}
		}

		private async Task<bool> isAdmin()
		{
			HttpContext httpContext = this._httpContextAccessor.HttpContext;
			AuthorizationResult isAdmin = await this._authorizationService.AuthorizeAsync(httpContext.User, "Admin");
			return isAdmin.Succeeded;
		}

		private IUrlHelper BuildUrlHelper()
		{
			IUrlHelperFactory factory = this._httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IUrlHelperFactory>();
			return factory.GetUrlHelper(this._actionContextAccessor.ActionContext);
		}
	}
}