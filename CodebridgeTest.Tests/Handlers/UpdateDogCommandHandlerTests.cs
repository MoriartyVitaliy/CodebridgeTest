using CodebridgeTest.Application.Dog.Commands;
using CodebridgeTest.Application.Dog.Handlers;
using CodebridgeTest.Core.Interfaces;
using FluentAssertions;
using MediatR;
using Moq;

namespace CodebridgeTest.Tests.Handlers
{
    public class UpdateDogCommandHandlerTests
    {
        private readonly Mock<IDogRepository> _repositoryMock;
        private readonly UpdateDogCommandHandler _handler;

        public UpdateDogCommandHandlerTests()
        {
            _repositoryMock = new Mock<IDogRepository>();
            _handler = new UpdateDogCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Update_Dog_When_Exists()
        {
            // Arrange
            var existingDog = new Core.Models.Dog
            {
                Name = "Rex",
                Color = "Black",
                TailLength = 10,
                Weight = 20
            };

            _repositoryMock.Setup(r => r.GetByNameAsync("Rex"))
                .ReturnsAsync(existingDog);

            var command = new UpdateDogCommand("Rex", "Brown", 15, 25);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);
            existingDog.Color.Should().Be("Brown");
            existingDog.TailLength.Should().Be(15);
            existingDog.Weight.Should().Be(25);

            _repositoryMock.Verify(r => r.UpdateAsync(existingDog), Times.Once);
            _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_When_Dog_Not_Found()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetByNameAsync("Unknown"))
                .ReturnsAsync((Core.Models.Dog?)null);

            var command = new UpdateDogCommand("Unknown", "Gray", 5, 10);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("Dog 'Unknown' not found");

            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Core.Models.Dog>()), Times.Never);
        }

        [Theory]
        [InlineData(-1, 10)]
        [InlineData(5, -3)]
        [InlineData(-2, -2)]
        public async Task Handle_Should_Throw_When_Invalid_Numbers(int tailLength, int weight)
        {
            // Arrange
            var command = new UpdateDogCommand("Rex", "Black", tailLength, weight);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Tail length and weight must be non-negative numbers.");

            _repositoryMock.Verify(r => r.GetByNameAsync(It.IsAny<string>()), Times.Never);
        }
    }
}
