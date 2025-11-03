using CodebridgeTest.Application.Dog.Queries;
using CodebridgeTest.Core.Common.Pagination;
using CodebridgeTest.Core.Interfaces;
using MediatR;

namespace CodebridgeTest.Application.Dog.Handlers
{
    public class GetAllDogsQueryHandler
    : IRequestHandler<GetAllDogsQuery, PagedResult<Core.Models.Dog>>
    {
        private readonly IDogRepository _dogRepository;
        private readonly ISortService<Core.Models.Dog> _sortService;
        private readonly IPaginationService _paginationService;

        public GetAllDogsQueryHandler(
            IDogRepository dogRepository,
            ISortService<Core.Models.Dog> sortService,
            IPaginationService paginationService)
        {
            _dogRepository = dogRepository;
            _sortService = sortService;
            _paginationService = paginationService;
        }

        public async Task<PagedResult<Core.Models.Dog>> Handle(GetAllDogsQuery request, CancellationToken cancellationToken)
        {
            var pagination = request.Pagination;
            var allowed = new[] { "name", "color", "tail_length", "weight" };

            var dogsQuery = _dogRepository.GetDogsQueryable();

            dogsQuery = _sortService.ApplySorting(dogsQuery, pagination.Attribute, pagination.Order, allowed);

            var result = await _paginationService.ApplyPaginationAsync(dogsQuery, pagination);

            return result;
        }
    }
}
