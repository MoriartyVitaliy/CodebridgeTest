namespace CodebridgeTest.Core.Exceptions
{
    public class RateLimitExceededException : Exception
    {
        public RateLimitExceededException(string message = "Too many requests") : base(message)
        {
        }
    }
}
