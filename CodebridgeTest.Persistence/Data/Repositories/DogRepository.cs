using CodebridgeTest.Core.Interfaces;
using CodebridgeTest.Core.Models;

namespace CodebridgeTest.Persistence.Data.Repositories
{
    public class DogRepository : IDogRepository
    {
        private readonly AppDbContext _context;

        public DogRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<Dog> GetDogsQueryable()
        {
            return _context.Dogs.AsQueryable();
        }
    }
}
