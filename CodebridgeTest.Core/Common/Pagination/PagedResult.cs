namespace CodebridgeTest.Core.Common.Pagination
{
    public class PagedResult<T>
    {
        public IEnumerable<T> items { get; set; } = Enumerable.Empty<T>();
        public int totalItems { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int totalPages => (int)Math.Ceiling((double)totalItems / pageSize);
        public static PagedResult<T> Create(
            IEnumerable<T> items,
            int totalItems,
            int pageNumber,
            int pageSize)
        {
            return new PagedResult<T>
            {
                items = items,
                totalItems = totalItems,
                pageNumber = pageNumber,
                pageSize = pageSize
            };
        }
    }
}
