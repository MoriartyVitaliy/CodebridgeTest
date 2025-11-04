using CodebridgeTest.Core.Models;

namespace CodebridgeTest.Core.Interfaces
{
    public interface IDogRepository
    {
        IQueryable<Dog> GetDogsQueryable();
        Task<Dog?> GetByNameAsync(string name);
        Task AddAsync(Dog dog);
        Task DeleteByNameAsync(string name);
        Task SaveChangesAsync();
    }
}
