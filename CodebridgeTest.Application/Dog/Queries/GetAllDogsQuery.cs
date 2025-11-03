using CodebridgeTest.Core.Common.Pagination;
using MediatR;

namespace CodebridgeTest.Application.Dog.Queries
{
    public record GetAllDogsQuery(PaginationRequest Pagination)
        : IRequest<PagedResult<Core.Models.Dog>>;
}
