using CodebridgeTest.Application.Dog.Commands;
using CodebridgeTest.Core.Interfaces;
using MediatR;

namespace CodebridgeTest.Application.Dog.Handlers
{
    public class CreateDogCommandHandler : IRequestHandler<CreateDogCommand, Unit>
    {
        private readonly IDogRepository _repository;

        public CreateDogCommandHandler(IDogRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(CreateDogCommand request, CancellationToken cancellationToken)
        {
            if (request.TailLength < 0 || request.Weight < 0)
                throw new ArgumentException("Tail length and weight must be non-negative numbers.");

            var existing = await _repository.GetByNameAsync(request.Name);
            if (existing != null)
                throw new InvalidOperationException("Dog with this name already exists.");

            var dog = new Core.Models.Dog
            {
                Name = request.Name,
                Color = request.Color,
                TailLength = request.TailLength,
                Weight = request.Weight
            };

            await _repository.AddAsync(dog);
            await _repository.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
