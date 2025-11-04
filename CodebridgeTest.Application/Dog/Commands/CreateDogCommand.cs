using MediatR;

namespace CodebridgeTest.Application.Dog.Commands
{
    public record CreateDogCommand(string Name, string Color, int TailLength, int Weight) 
        : IRequest<Unit>;
}
