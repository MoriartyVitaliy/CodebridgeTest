using CodebridgeTest.Core.Common.Result;

namespace CodebridgeTest.Core.Interfaces
{
    public interface ICrudRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task<OperationResult<T>> AddAsync(T entity);
        Task<OperationResult<T>> UpdateAsync(T entity);
        Task<OperationResult> DeleteAsync(Guid id);
    }
}
