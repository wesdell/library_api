using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace library_api.Utils
{
	public class HATEOASFilterAttribute : ResultFilterAttribute
	{
		protected bool InlcudeHATEOAS(ResultExecutingContext context)
		{
			ObjectResult result = context.Result as ObjectResult;
			if (!this.IsSuccessfulResponse(result))
			{
				return false;
			}

			StringValues header = context.HttpContext.Request.Headers["includeHATEOAS"];
			if (header.Count == 0)
			{
				return false;
			}

			string headerValue = header[0];
			if (!headerValue.Equals("yes", StringComparison.InvariantCultureIgnoreCase))
			{
				return false;
			}

			return true;
		}

		private bool IsSuccessfulResponse(ObjectResult objectResult)
		{
			if (objectResult == null || objectResult.Value == null)
			{
				return false;
			}
			if (objectResult.StatusCode.HasValue && !objectResult.StatusCode.Value.ToString().StartsWith("2"))
			{
				return false;
			}
			return true;
		}
	}
}