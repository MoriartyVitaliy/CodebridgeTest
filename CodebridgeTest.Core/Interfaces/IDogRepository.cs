using CodebridgeTest.Core.Models;

namespace CodebridgeTest.Core.Interfaces
{
    public interface IDogRepository
    {
        IQueryable<Dog> GetAll();
    }
}
