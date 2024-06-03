using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace library_api.Utils
{
	public class HATEOASParameter : IOperationFilter
	{
		public void Apply(OpenApiOperation operation, OperationFilterContext context)
		{
			if (context.ApiDescription.HttpMethod != HttpMethod.Get.ToString())
			{
				return;
			}

			if (operation.Parameters == null)
			{
				operation.Parameters = new List<OpenApiParameter>();
			}

			operation.Parameters.Add(
				new OpenApiParameter
				{
					Name = "includeHATEOAS",
					In = ParameterLocation.Header,
					Required = false
				}
			);
		}
	}
}
