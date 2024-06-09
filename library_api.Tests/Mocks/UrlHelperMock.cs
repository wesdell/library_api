using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace library_api.Tests.Mocks
{
	public class UrlHelperMock
	{
		public IUrlHelper Object { get; private set; }

		public UrlHelperMock()
		{
			Mock<IUrlHelper> mock = new Mock<IUrlHelper>();
			mock.Setup(opt => opt.Link(
					It.IsAny<string>(),
					It.IsAny<object>()
				)).Returns(string.Empty);
			this.Object = mock.Object;
		}
	}
}
