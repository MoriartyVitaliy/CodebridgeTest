namespace CodebridgeTest.Core.Exceptions
{
    public class ValidationException : ArgumentException
    {
        public ValidationException(string message) : base(message)
        {
        }
    }
}
