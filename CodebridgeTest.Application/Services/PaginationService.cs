using CodebridgeTest.Core.Common.Pagination;
using CodebridgeTest.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CodebridgeTest.Application.Services
{
    public class PaginationService : IPaginationService
    {
        public IQueryable<T> ApplyPagination<T>(IQueryable<T> query, PaginationRequest pagination)
        {
            return query
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize);
        }

        public async Task<PagedResult<T>> ApplyPaginationAsync<T>(IQueryable<T> query, PaginationRequest pagination)
        {
            var totalItems = await query.CountAsync();
            var pagedItems = await ApplyPagination(query, pagination).ToListAsync();

            return new PagedResult<T>
            {
                items = pagedItems,
                totalItems = totalItems,
                pageNumber = pagination.PageNumber,
                pageSize = pagination.PageSize
            };
        }
    }
}
