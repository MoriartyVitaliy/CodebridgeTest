namespace CodebridgeTest.Core.Exceptions
{
    public class ResourceNotFoundException : KeyNotFoundException
    {
        public ResourceNotFoundException(string resourceName, object? key = null)
            : base(key != null
                ? $"{resourceName} with key '{key}' was not found."
                : $"{resourceName} was not found.")
        {
        }
    }
}
