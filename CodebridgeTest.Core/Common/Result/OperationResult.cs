namespace CodebridgeTest.Core.Common.Result
{
    public class OperationResult
    {
        public bool IsSuccess { get; protected set; }
        public string? ErrorMessage { get; protected set; }

        public static OperationResult Success() =>
            new OperationResult { IsSuccess = true };

        public static OperationResult Failure(string errorMessage) =>
            new OperationResult { IsSuccess = false, ErrorMessage = errorMessage };
    }

    public class OperationResult<T> : OperationResult
    {
        public T? Data { get; protected set; }

        public static OperationResult<T> Success(T data) =>
            new OperationResult<T> { IsSuccess = true, Data = data };

        public static OperationResult<T> Failure(string errorMessage) =>
            new OperationResult<T> { IsSuccess = false, ErrorMessage = errorMessage };
    }
}
