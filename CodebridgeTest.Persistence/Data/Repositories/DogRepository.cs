using CodebridgeTest.Core.Interfaces;
using CodebridgeTest.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CodebridgeTest.Persistence.Data.Repositories
{
    public class DogRepository : IDogRepository
    {
        private readonly AppDbContext _context;

        public DogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Dog dog)
        {
            await _context.Dogs.AddAsync(dog);
        }

        public async Task<Dog?> GetByNameAsync(string name)
        {
            return await _context.Dogs.FirstOrDefaultAsync(x => x.Name == name);
        }

        public IQueryable<Dog> GetDogsQueryable()
        {
            return _context.Dogs.AsQueryable();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task DeleteByNameAsync(string name)
        {
            var dog = await _context.Dogs.FirstOrDefaultAsync(x => x.Name == name);

            if (dog == null)
                throw new KeyNotFoundException($"Dog '{name}' not found");

            _context.Dogs.Remove(dog);
            await _context.SaveChangesAsync();
        }
    }
}
