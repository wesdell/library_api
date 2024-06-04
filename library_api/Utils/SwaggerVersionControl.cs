using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace library_api.Utils
{
	public class SwaggerVersionControl : IControllerModelConvention
	{
		public void Apply(ControllerModel controller)
		{
			string controllerNamespace = controller.ControllerType.Namespace;
			string apiVersion = controllerNamespace.Split('.').Last().ToLower();
			controller.ApiExplorer.GroupName = apiVersion;
		}
	}
}