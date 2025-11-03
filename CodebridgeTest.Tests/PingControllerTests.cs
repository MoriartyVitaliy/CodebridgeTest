using CodebridgeTest.Controllers;
using CodebridgeTest.Core.Common.Info;
using Microsoft.AspNetCore.Mvc;

namespace CodebridgeTest.Tests
{
    public class PingControllerTests
    {
        [Fact]
        public void Get_Should_Return_Version_String()
        {
            var controller = new PingController();

            var result = controller.Get() as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result!.StatusCode);
            Assert.Equal(ApiInfo.FullVersion, result.Value);
        }
    }
}