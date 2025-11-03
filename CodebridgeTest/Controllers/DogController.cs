using CodebridgeTest.Application.Dog.Queries;
using CodebridgeTest.Core.Common.Pagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CodebridgeTest.Controllers
{
    [ApiController]
    public class DogController : ControllerBase
    {
        private readonly IMediator _mediator;
        public DogController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("dogs")]
        public async Task<IActionResult> GetDogs([FromQuery] PaginationRequest pagination)
        {
            var result = await _mediator.Send(new GetAllDogsQuery(pagination));
            return Ok(result);
        }
        [HttpPost("dog")]
        public IActionResult AddDog()
        {
            return Ok("Not implemented yet");
        }
    }
}
