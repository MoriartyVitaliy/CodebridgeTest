using CodebridgeTest.Application.Dog.Commands;
using CodebridgeTest.Core.Interfaces;
using MediatR;

namespace CodebridgeTest.Application.Dog.Handlers
{
    public class DeleteDogCommandHandler : IRequestHandler<DeleteDogCommand, Unit>
    {
        private readonly IDogRepository _repository;

        public DeleteDogCommandHandler(IDogRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(DeleteDogCommand request, CancellationToken cancellationToken)
        {
            await _repository.DeleteByNameAsync(request.Name);
            return Unit.Value;
        }
    }
}
