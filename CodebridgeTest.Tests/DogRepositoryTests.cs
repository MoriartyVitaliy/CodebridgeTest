using CodebridgeTest.Core.Models;
using CodebridgeTest.Persistence.Data;
using CodebridgeTest.Persistence.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;


namespace CodebridgeTest.Tests
{
    public class DogRepositoryTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task GetDogsQueryable_Should_Return_IQueryable_And_Contain_Data()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new DogRepository(context);

            context.Dogs.AddRange(
                new Dog { Name = "Rex", Color = "Black", TailLength = 10, Weight = 20 },
                new Dog { Name = "Bella", Color = "White", TailLength = 7, Weight = 18 }
            );
            await context.SaveChangesAsync();

            // Act
            var query = repo.GetDogsQueryable();

            // Assert - Check type is IQueryable
            Assert.IsAssignableFrom<IQueryable<Dog>>(query);

            // Assert - Check data can be read
            var dogs = query.ToList();
            Assert.Equal(2, dogs.Count);

            // Assert - Check a value exists
            Assert.Contains(dogs, d => d.Name == "Rex");
            Assert.Contains(dogs, d => d.Name == "Bella");
        }

        [Fact]
        public void GetDogsQueryable_Should_Return_Empty_When_No_Data()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new DogRepository(context);

            // Act
            var query = repo.GetDogsQueryable();
            var dogs = query.ToList();

            // Assert
            Assert.Empty(dogs);
        }

        [Fact]
        public async Task DeleteByNameAsync_Should_Delete_Existing_Dog()
        {
            var ctx = GetDbContext();
            var repo = new DogRepository(ctx);

            var dog = new Dog { Name = "Buddy", Color = "Black", TailLength = 10, Weight = 20 };

            await repo.AddAsync(dog);
            await repo.SaveChangesAsync();

            await repo.DeleteByNameAsync("Buddy");

            Assert.Null(await ctx.Dogs.FirstOrDefaultAsync(d => d.Name == "Buddy"));
        }

        [Fact]
        public async Task DeleteByNameAsync_Should_Throw_If_Not_Found()
        {
            var ctx = GetDbContext();
            var repo = new DogRepository(ctx);

            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                repo.DeleteByNameAsync("UnknownDog"));
        }
    }
}
