using library_api.DTOs;
using library_api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace library_api.Utils
{
	public class HATEOASAuthorFilterAttribute : HATEOASFilterAttribute
	{
		private readonly SetHATEOASLinks _setHATEOASLinks;

		public HATEOASAuthorFilterAttribute(SetHATEOASLinks setHATEOASLinks)
		{
			this._setHATEOASLinks = setHATEOASLinks;
		}

		public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
		{
			bool includeHATEOAS = this.InlcudeHATEOAS(context);
			if (!includeHATEOAS)
			{
				await next();
				return;
			}

			ObjectResult objectResult = context.Result as ObjectResult;
			AuthorDTO authorDTO = objectResult.Value as AuthorDTO;
			if (authorDTO == null)
			{
				List<AuthorDTO> authorDTOs = objectResult.Value as List<AuthorDTO> ?? throw new ArgumentNullException("An AuhtorDTO instance was expected.");
				authorDTOs.ForEach(async author => await this._setHATEOASLinks.Set(author));
				objectResult.Value = authorDTOs;
			}
			else
			{
				await this._setHATEOASLinks.Set(authorDTO);
			}

			await next();
		}
	}
}