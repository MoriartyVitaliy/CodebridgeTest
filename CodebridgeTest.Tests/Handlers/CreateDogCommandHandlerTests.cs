using CodebridgeTest.Application.Dog.Commands;
using CodebridgeTest.Application.Dog.Handlers;
using CodebridgeTest.Core.Interfaces;
using Moq;

namespace CodebridgeTest.Tests.Handlers
{
    public class CreateDogCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Throw_When_TailLength_Is_Negative()
        {
            var repo = new Mock<IDogRepository>();
            var handler = new CreateDogCommandHandler(repo.Object);

            var command = new CreateDogCommand(
                Name: "TestDog",
                Color: "Black",
                TailLength: -1,
                Weight: 10
            );

            await Assert.ThrowsAsync<ArgumentException>(() =>
                handler.Handle(command, default));
        }

        [Fact]
        public async Task Handle_Should_Throw_When_Weight_Is_Negative()
        {
            var repo = new Mock<IDogRepository>();
            var handler = new CreateDogCommandHandler(repo.Object);

            var command = new CreateDogCommand(
                Name: "TestDog",
                Color: "Brown",
                TailLength: 5,
                Weight: -10
            );

            await Assert.ThrowsAsync<ArgumentException>(() =>
                handler.Handle(command, default));
        }

        [Fact]
        public async Task Handle_Should_Throw_When_Dog_With_Same_Name_Exists()
        {
            var repo = new Mock<IDogRepository>();
            repo.Setup(r => r.GetByNameAsync("Rex"))
                .ReturnsAsync(new Core.Models.Dog());

            var handler = new CreateDogCommandHandler(repo.Object);

            var command = new CreateDogCommand(
                Name: "Rex",
                Color: "White",
                TailLength: 10,
                Weight: 22
            );

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.Handle(command, default));
        }

        [Fact]
        public async Task Handle_Should_Create_Dog_When_Data_Is_Valid()
        {
            var repo = new Mock<IDogRepository>();

            repo.Setup(r => r.GetByNameAsync("Buddy"))
                .ReturnsAsync((Core.Models.Dog?)null);

            var handler = new CreateDogCommandHandler(repo.Object);

            var command = new CreateDogCommand(
                Name: "Buddy",
                Color: "Black",
                TailLength: 8,
                Weight: 25
            );

            await handler.Handle(command, default);

            repo.Verify(r => r.AddAsync(It.IsAny<Core.Models.Dog>()), Times.Once);
            repo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }
    }
}
