using CodebridgeTest.Application.Dog.Commands;
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
        public async Task<IActionResult> AddDog([FromBody] CreateDogCommand command)
        {
            await _mediator.Send(command);
            return StatusCode(StatusCodes.Status201Created, new
            {
                message = "Dog successfully created"
            });
        }
        [HttpDelete("dog/{name}")]
        public async Task<IActionResult> DeleteDog(string name)
        {
            await _mediator.Send(new DeleteDogCommand(name));

            return Ok(new
            {
                message = $"Dog '{name}' successfully deleted"
            });
        }
    }
}
