using Microsoft.AspNetCore.Authorization;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace library_api.Tests.Mocks
{
	public class AuthorizationServiceMock
	{
		public AuthorizationResult Result { get; private set; }
		public IAuthorizationService Object { get; private set; }

		public AuthorizationServiceMock(AuthorizationResult result)
		{
			this.Result = result;
			Mock<IAuthorizationService> mock = new Mock<IAuthorizationService>();
			mock.Setup(opt => opt.AuthorizeAsync(
					It.IsAny<ClaimsPrincipal>(),
					It.IsAny<object>(),
					It.IsAny<IEnumerable<IAuthorizationRequirement>>()
				)).Returns(Task.FromResult(this.Result));
			mock.Setup(opt => opt.AuthorizeAsync(
					It.IsAny<ClaimsPrincipal>(),
					It.IsAny<object>(),
					It.IsAny<string>()
				)).Returns(Task.FromResult(this.Result));
			this.Object = mock.Object;
		}
	}
}
