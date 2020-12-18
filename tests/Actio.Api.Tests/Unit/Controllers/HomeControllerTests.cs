using Actio.Api.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Actio.Api.Tests.Unit.Controllers
{
    public class HomeControllerTests
    {

        [Fact]
        public void Home_Controller_Get_Returns_String_Content()
        {
            var result = (new HomeController()).Get();

            var contentResult = result as ContentResult;

            contentResult.Should().NotBeNull();
            contentResult.Content.Should().Be("Hello from actio API");
        }
    }
}
