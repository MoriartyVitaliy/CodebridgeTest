using System.Linq.Expressions;

namespace CodebridgeTest.Core.Interfaces
{
    public interface ISortService<T>
    {
        IQueryable<T> ApplySorting(IQueryable<T> query, string? attribute, string? order, string[] allowedAttributes);
        Expression<Func<T, object>> GetSortExpression(string property);
    }
}
