using CodebridgeTest.Application.Dog.Handlers;
using CodebridgeTest.Application.Dog.Queries;
using CodebridgeTest.Core.Interfaces;
using Moq;

namespace CodebridgeTest.Tests.Handlers
{
    public class GetDogByNameQueryHandlerTests
    {
        private readonly Mock<IDogRepository> _repositoryMock;
        private readonly GetDogByNameQueryHandler _handler;

        public GetDogByNameQueryHandlerTests()
        {
            _repositoryMock = new Mock<IDogRepository>();
            _handler = new GetDogByNameQueryHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnDog_WhenDogExists()
        {
            var dog = new Core.Models.Dog
            {
                Name = "Rex",
                Color = "Brown",
                TailLength = 15,
                Weight = 25
            };

            _repositoryMock
                .Setup(x => x.GetByNameAsync("Rex"))
                .ReturnsAsync(dog);

            var query = new GetDogByNameQuery("Rex");

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal("Rex", result!.Name);
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenDogDoesNotExist()
        {
            _repositoryMock
                .Setup(x => x.GetByNameAsync("Unknown"))
                .ReturnsAsync((Core.Models.Dog?)null);

            var query = new GetDogByNameQuery("Unknown");

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Null(result);
        }
    }
}
