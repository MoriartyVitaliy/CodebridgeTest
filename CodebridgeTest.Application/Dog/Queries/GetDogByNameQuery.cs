using MediatR;

namespace CodebridgeTest.Application.Dog.Queries
{
    public record GetDogByNameQuery(string Name) : IRequest<Core.Models.Dog?>;
}
