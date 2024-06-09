using library_api.Controllers.V1;
using library_api.DTOs;
using library_api.Tests.Mocks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace library_api.Tests.UnitTests
{
	[TestClass]
	public class RootControllerTests
	{
		[TestMethod]
		public async Task SuccessOnUserIsNotAdminGet2Links()
		{
			AuthorizationServiceMock authorizationServiceMock = new AuthorizationServiceMock(AuthorizationResult.Failed());
			UrlHelperMock urlHelperMock = new UrlHelperMock();

			RootController rootController = new RootController(authorizationServiceMock.Object);
			rootController.Url = urlHelperMock.Object;

			ActionResult<IEnumerable<HATEOASData>> testResult = await rootController.Get();

			Assert.AreEqual(2, testResult?.Value?.Count());
		}

		[TestMethod]
		public async Task SuccessOnUserIsAdminGet4Links()
		{
			AuthorizationServiceMock authorizationServiceMock = new AuthorizationServiceMock(AuthorizationResult.Success());
			UrlHelperMock urlHelperMock = new UrlHelperMock();

			RootController rootController = new RootController(authorizationServiceMock.Object);
			rootController.Url = urlHelperMock.Object;

			ActionResult<IEnumerable<HATEOASData>> testResult = await rootController.Get();

			Assert.AreEqual(3, testResult?.Value?.Count());
		}
	}
}
