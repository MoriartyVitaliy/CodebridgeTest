using CodebridgeTest.Core.Interfaces;
using CodebridgeTest.Core.Models;

namespace CodebridgeTest.Persistence.Data.Repositories
{
    public class DogRepository : IDogRepository
    {
        public IQueryable<Dog> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
