using CodebridgeTest.Application.Dog.Commands;
using CodebridgeTest.Application.Dog.Handlers;
using CodebridgeTest.Core.Interfaces;
using Moq;

namespace CodebridgeTest.Tests
{
    public class DeleteDogCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Call_Repository_When_Dog_Exists()
        {
            var repoMock = new Mock<IDogRepository>();
            repoMock.Setup(r => r.DeleteByNameAsync("Rex"))
                    .Returns(Task.CompletedTask);

            var handler = new DeleteDogCommandHandler(repoMock.Object);
            var command = new DeleteDogCommand("Rex");

            await handler.Handle(command, default);

            repoMock.Verify(r => r.DeleteByNameAsync("Rex"), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_When_Dog_Does_Not_Exist()
        {
            var repoMock = new Mock<IDogRepository>();
            repoMock.Setup(r => r.DeleteByNameAsync("Ghost"))
                    .ThrowsAsync(new KeyNotFoundException());

            var handler = new DeleteDogCommandHandler(repoMock.Object);
            var command = new DeleteDogCommand("Ghost");

            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                handler.Handle(command, default));
        }
    }
}
