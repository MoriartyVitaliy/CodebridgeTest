using CodebridgeTest.Application.Dog.Queries;
using CodebridgeTest.Controllers;
using CodebridgeTest.Core.Common.Pagination;
using CodebridgeTest.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;


using Moq;


namespace CodebridgeTest.Tests
{
    public class DogControllerTests
    {

        [Fact]
        public async Task GetDogs_Should_Return_Ok_With_Data()
        {
            var mediatorMock = new Mock<IMediator>();
            var pagination = new PaginationRequest { PageNumber = 1, PageSize = 10 };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllDogsQuery>(), default))
                .ReturnsAsync(new PagedResult<Dog>());

            var controller = new DogController(mediatorMock.Object);

            var result = await controller.GetDogs(pagination);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<PagedResult<Dog>>(ok.Value);
        }

        [Fact]
        public async Task GetDogs_Should_Return_Seeded_List()
        {
            var mediatorMock = new Mock<IMediator>();

            var expectedDogs = new List<Dog>
            {
                new Dog { Name = "Neo", Color = "red&amber", TailLength = 22, Weight = 32 },
                new Dog { Name = "Jessy", Color = "black&white", TailLength = 7, Weight = 14 }
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllDogsQuery>(), default))
                .ReturnsAsync(new PagedResult<Dog>
                {
                    items = expectedDogs,
                    totalItems = 2
                });

            var controller = new DogController(mediatorMock.Object);

            var result = await controller.GetDogs(new PaginationRequest());
            var ok = Assert.IsType<OkObjectResult>(result);

            var response = Assert.IsType<PagedResult<Dog>>(ok.Value);
            var dogs = response.items.ToList();

            Assert.Equal(2, dogs.Count);
            Assert.Equal("Neo", dogs[0].Name);
            Assert.Equal("Jessy", dogs[1].Name);
        }

        [Fact]
        public async Task GetDogs_Should_Pass_Sorting_Params_To_Query()
        {
            var mediatorMock = new Mock<IMediator>();

            var pagination = new PaginationRequest
            {
                Attribute = "weight",
                Order = "desc"
            };

            mediatorMock
                .Setup(m => m.Send(It.Is<GetAllDogsQuery>(q =>
                    q.Pagination.Attribute == "weight" &&
                    q.Pagination.Order == "desc"
                ), default))
                .ReturnsAsync(new PagedResult<Dog>());

            var controller = new DogController(mediatorMock.Object);

            await controller.GetDogs(pagination);

            mediatorMock.Verify(m => m.Send(It.IsAny<GetAllDogsQuery>(), default), Times.Once);
        }


        [Fact]
        public async Task GetDogs_Should_Pass_Pagination_Params_To_Query()
        {
            var mediatorMock = new Mock<IMediator>();

            var pagination = new PaginationRequest
            {
                PageNumber = 3,
                PageSize = 10
            };

            mediatorMock
                .Setup(m => m.Send(It.Is<GetAllDogsQuery>(q =>
                    q.Pagination.PageNumber == 3 &&
                    q.Pagination.PageSize == 10
                ), default))
                .ReturnsAsync(new PagedResult<Dog>());

            var controller = new DogController(mediatorMock.Object);

            await controller.GetDogs(pagination);

            mediatorMock.Verify(m => m.Send(It.IsAny<GetAllDogsQuery>(), default), Times.Once);
        }
    }
}
