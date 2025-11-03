using CodebridgeTest.Core.Interfaces;
using System.Linq.Expressions;

namespace CodebridgeTest.Application.Services
{
    public class DogSortService : ISortService<Core.Models.Dog>
    {
        public IQueryable<Core.Models.Dog> ApplySorting(IQueryable<Core.Models.Dog> query, string? attribute, string? order, string[] allowedAttributes)
        {
            var attr = allowedAttributes.Contains(attribute?.ToLower())
                ? attribute.ToLower()
                : "name";

            var sortExpression = GetSortExpression(attr);

            return order?.ToLower() == "desc"
                ? query.OrderByDescending(sortExpression)
                : query.OrderBy(sortExpression);
        }

        public Expression<Func<Core.Models.Dog, object>> GetSortExpression(string property)
        {
            return property.ToLower() switch
            {
                "name" => dog => dog.Name,
                "color" => dog => dog.Color,
                "tail_length" => dog => dog.TailLength,
                "weight" => dog => dog.Weight,
                _ => dog => dog.Name
            };
        }
    }
}