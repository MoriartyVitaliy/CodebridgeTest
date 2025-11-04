using MediatR;

namespace CodebridgeTest.Application.Dog.Commands
{
    public record DeleteDogCommand(string Name) 
        : IRequest<Unit>;
}
