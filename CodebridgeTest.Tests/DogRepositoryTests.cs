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
            await repo.SaveChangesAsync();

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

        [Fact]
        public async Task UpdateAsync_Should_Update_Existing_Dog()
        {
            var context = GetDbContext();
            var repository = new DogRepository(context);

            var dog = new Dog
            {
                Name = "Buddy",
                Color = "Brown",
                TailLength = 5,
                Weight = 20
            };

            await repository.AddAsync(dog);
            await repository.SaveChangesAsync();

            dog.Color = "Black";
            dog.TailLength = 7;
            dog.Weight = 23;

            await repository.UpdateAsync(dog);
            await repository.SaveChangesAsync();

            var updatedDog = await repository.GetByNameAsync("Buddy");

            Assert.NotNull(updatedDog);
            Assert.Equal("Black", updatedDog.Color);
            Assert.Equal(7, updatedDog.TailLength);
            Assert.Equal(23, updatedDog.Weight);
        }

        [Fact]
        public async Task UpdateAsync_Should_Throw_When_Dog_Not_Found()
        {
            var context = GetDbContext();
            var repository = new DogRepository(context);

            var dog = new Dog
            {
                Name = "Ghost",
                Color = "White",
                TailLength = 3,
                Weight = 12
            };

            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await repository.UpdateAsync(dog);
            });
        }

        [Fact]
        public async Task GetByNameAsync_Should_Return_Dog_When_Exists()
        {
            var context = GetDbContext();
            var repository = new DogRepository(context);

            var dog = new Dog
            {
                Name = "Rex",
                Color = "Brown",
                TailLength = 10,
                Weight = 25
            };

            await repository.AddAsync(dog);
            await repository.SaveChangesAsync();

            var result = await repository.GetByNameAsync("Rex");

            Assert.NotNull(result);
            Assert.Equal("Rex", result!.Name);
            Assert.Equal("Brown", result.Color);
            Assert.Equal(10, result.TailLength);
            Assert.Equal(25, result.Weight);
        }

        [Fact]
        public async Task GetByNameAsync_Should_Return_Null_When_NotFound()
        {
            var context = GetDbContext();
            var repository = new DogRepository(context);

            var result = await repository.GetByNameAsync("UnknownDog");

            Assert.Null(result);
        }
    }
}
