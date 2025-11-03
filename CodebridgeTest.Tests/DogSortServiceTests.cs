using CodebridgeTest.Application.Services;
using CodebridgeTest.Core.Models;

namespace CodebridgeTest.Tests
{
    public class DogSortServiceTests
    {
        [Fact]
        public void ApplySorting_Should_Sort_Seeded_Dogs_By_Name_Ascending()
        {
            var service = new DogSortService();

            var dogs = new List<Dog>
            {
                new Dog { Name = "Neo", Color = "red&amber", TailLength = 22, Weight = 32 },
                new Dog { Name = "Jessy", Color = "black&white", TailLength = 7, Weight = 14 }
            }.AsQueryable();

            var sorted = service.ApplySorting(dogs, null, null, new[] { "name", "color", "tail_length", "weight" }).ToList();

            Assert.Equal("Jessy", sorted.First().Name);
            Assert.Equal("Neo", sorted.Last().Name);
        }

        [Fact]
        public void ApplySorting_Should_Sort_Seeded_Dogs_By_Weight_Desc()
        {
            var service = new DogSortService();

            var dogs = new List<Dog>
            {
                new Dog { Name = "Neo", Color = "red&amber", TailLength = 22, Weight = 32 },
                new Dog { Name = "Jessy", Color = "black&white", TailLength = 7, Weight = 14 }
            }.AsQueryable();

            var sorted = service.ApplySorting(dogs, "weight", "desc", new[] { "name", "color", "tail_length", "weight" }).ToList();

            Assert.Equal(32, sorted.First().Weight);
            Assert.Equal(14, sorted.Last().Weight);
        }

    }
}
