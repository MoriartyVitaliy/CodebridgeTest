using CodebridgeTest.Core.Common.Info;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodebridgeTest.Controllers
{
    [Route("ping")]
    [ApiController]
    public class PingController : ControllerBase
    {
        private readonly ILogger<PingController> _logger;

        public PingController(ILogger<PingController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            _logger.LogInformation("Ping endpoint called at {Time}", DateTime.UtcNow);

            return Ok(ApiInfo.FullVersion);
        }
    }
}
