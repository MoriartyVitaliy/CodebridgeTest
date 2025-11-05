using CodebridgeTest.Application.Dog.Queries;
using CodebridgeTest.Core.Interfaces;
using MediatR;

namespace CodebridgeTest.Application.Dog.Handlers
{
    public class GetDogByNameQueryHandler : IRequestHandler<GetDogByNameQuery, Core.Models.Dog?>
    {
        private readonly IDogRepository _repository;

        public GetDogByNameQueryHandler(IDogRepository repository)
        {
            _repository = repository;
        }

        public async Task<Core.Models.Dog?> Handle(GetDogByNameQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetByNameAsync(request.Name);
        }
    }
}