using Microsoft.EntityFrameworkCore;

namespace library_api.Utils
{
	public static class HttpContextExtensions
	{
		public async static Task InsertPaginationParametersInHeader<T>(this HttpContext httpContext, IQueryable<T> queryable)
		{
			ArgumentNullException.ThrowIfNull(httpContext);

			double amount = await queryable.CountAsync();
			httpContext.Response.Headers.Append("recordsAmount", amount.ToString());
		}
	}
}