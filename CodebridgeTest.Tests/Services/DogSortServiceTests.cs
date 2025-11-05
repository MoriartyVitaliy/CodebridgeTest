using CodebridgeTest.Application.Services;
using CodebridgeTest.Core.Models;
using FluentAssertions;

namespace CodebridgeTest.Tests.Services
{
    public class DogSortServiceTests
    {
        private readonly DogSortService _service;
        private readonly string[] _allowed = { "name", "color", "tail_length", "weight" };

        public DogSortServiceTests()
        {
            _service = new DogSortService();
        }

        private List<Dog> GetMockDogs() => new()
        {
            new Dog { Name = "Charlie", Color = "Brown", TailLength = 10, Weight = 20 },
            new Dog { Name = "Buddy", Color = "Black", TailLength = 5, Weight = 10 },
            new Dog { Name = "Alpha", Color = "White", TailLength = 15, Weight = 30 },
        };

        [Fact]
        public void ShouldSortByDefault_NameAscending()
        {
            var dogs = GetMockDogs().AsQueryable();

            var result = _service.ApplySorting(dogs, null, null, _allowed).ToList();

            result.Select(x => x.Name).Should().ContainInOrder("Alpha", "Buddy", "Charlie");
        }

        [Fact]
        public void ShouldSortByTailLength_Ascending()
        {
            var dogs = GetMockDogs().AsQueryable();

            var result = _service.ApplySorting(dogs, "tail_length", "asc", _allowed).ToList();

            result.Select(x => x.TailLength).Should().ContainInOrder(5, 10, 15);
        }

        [Fact]
        public void ShouldSortByWeight_Descending()
        {
            var dogs = GetMockDogs().AsQueryable();

            var result = _service.ApplySorting(dogs, "weight", "desc", _allowed).ToList();

            result.Select(x => x.Weight).Should().ContainInOrder(30, 20, 10);
        }

        [Fact]
        public void ShouldSortByColor_Ascending()
        {
            var dogs = GetMockDogs().AsQueryable();

            var result = _service.ApplySorting(dogs, "color", "asc", _allowed).ToList();

            result.Select(x => x.Color).Should().ContainInOrder("Black", "Brown", "White");
        }

        [Fact]
        public void ShouldThrow_WhenInvalidAttribute()
        {
            var dogs = GetMockDogs().AsQueryable();

            Action act = () =>
                _service.ApplySorting(dogs, "height", "asc", _allowed);

            act.Should().Throw<Core.Exceptions.ValidationException>()
               .WithMessage("*height*not supported*");
        }

        [Fact]
        public void ShouldThrow_WhenGetSortExpressionInvalid()
        {
            Action act = () => _service.GetSortExpression("invalid");

            act.Should().Throw<Core.Exceptions.ValidationException>()
                .WithMessage("*invalid*");
        }
    }
}
