using CodebridgeTest.Core.Common.Pagination;

namespace CodebridgeTest.Core.Interfaces
{
    public interface IPaginationService
    {
        IQueryable<T> ApplyPagination<T>(IQueryable<T> query, PaginationRequest pagination);
        Task<PagedResult<T>> ApplyPaginationAsync<T>(IQueryable<T> query, PaginationRequest pagination);
    }
}
