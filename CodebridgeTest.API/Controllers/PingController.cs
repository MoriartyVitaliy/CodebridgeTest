using CodebridgeTest.Core.Common.Info;
using Microsoft.AspNetCore.Mvc;

namespace CodebridgeTest.API.Controllers
{
    [Route("ping")]
    [ApiController]
    public class PingController : ControllerBase
    {
        public PingController()
        {   }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(ApiInfo.FullVersion);
        }
    }
}
