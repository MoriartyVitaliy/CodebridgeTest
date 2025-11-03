using CodebridgeTest.Application.Services;
using CodebridgeTest.Core.Common.Pagination;


namespace CodebridgeTest.Tests
{
    public class PaginationServiceTests
    {
        [Fact]
        public void ApplyPagination_Should_Return_Correct_Page()
        {
            var service = new PaginationService();
            var data = Enumerable.Range(1, 10).AsQueryable();
            var request = new PaginationRequest { PageNumber = 2, PageSize = 3 };

            var resultQuery = service.ApplyPagination(data, request);
            var result = new PagedResult<int>
            {
                items = resultQuery.ToList(),
                totalItems = data.Count(),
                pageNumber = request.PageNumber,
                pageSize = request.PageSize
            };

            Assert.Equal(10, result.totalItems);
            Assert.Equal(new List<int> { 4, 5, 6 }, result.items);
        }
    }
}
