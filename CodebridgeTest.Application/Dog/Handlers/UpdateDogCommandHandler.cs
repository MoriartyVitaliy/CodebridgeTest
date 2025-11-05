using CodebridgeTest.Application.Dog.Commands;
using CodebridgeTest.Core.Interfaces;
using MediatR;

namespace CodebridgeTest.Application.Dog.Handlers
{
    public class UpdateDogCommandHandler : IRequestHandler<UpdateDogCommand, Unit>
    {
        private readonly IDogRepository _repository;

        public UpdateDogCommandHandler(IDogRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(UpdateDogCommand request, CancellationToken cancellationToken)
        {
            if (request.TailLength < 0 || request.Weight < 0)
                throw new ArgumentException("Tail length and weight must be non-negative numbers.");
            var existingDog = await _repository.GetByNameAsync(request.Name);
            if (existingDog is null)
                throw new KeyNotFoundException($"Dog '{request.Name}' not found");

            existingDog.Color = request.Color;
            existingDog.TailLength = request.TailLength;
            existingDog.Weight = request.Weight;

            await _repository.UpdateAsync(existingDog);
            await _repository.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
