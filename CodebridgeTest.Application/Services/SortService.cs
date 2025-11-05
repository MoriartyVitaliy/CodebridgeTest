using CodebridgeTest.Core.Interfaces;
using System.Linq.Expressions;
using CodebridgeTest.Core.Exceptions;

namespace CodebridgeTest.Application.Services
{
    public class DogSortService : ISortService<Core.Models.Dog>
    {
        public IQueryable<Core.Models.Dog> ApplySorting(
            IQueryable<Core.Models.Dog> query,
            string? attribute,
            string? order,
            string[] allowedAttributes)
        {
            if (!string.IsNullOrEmpty(attribute) && !allowedAttributes.Contains(attribute.ToLower()))
            {
                throw new ValidationException($"Sorting by '{attribute}' is not supported. Allowed fields: {string.Join(", ", allowedAttributes)}");
            }

            var attr = string.IsNullOrEmpty(attribute) ? "name" : attribute.ToLower();
            var sortExpression = GetSortExpression(attr);

            return order?.ToLower() == "desc"
                ? query.OrderByDescending(sortExpression)
                : query.OrderBy(sortExpression);
        }

        public Expression<Func<Core.Models.Dog, object>> GetSortExpression(string property)
        {
            return property switch
            {
                "name" => dog => dog.Name,
                "color" => dog => dog.Color,
                "tail_length" => dog => dog.TailLength,
                "weight" => dog => dog.Weight,
                _ => throw new ValidationException($"Sort attribute '{property}' is invalid.")
            };
        }
    }
}