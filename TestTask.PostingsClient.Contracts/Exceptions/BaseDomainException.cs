namespace TestTask.PostingsClient.Contracts.Exceptions
{
    public class BaseDomainException : Exception
    {
        public BaseDomainException(int statusCode) : base(string.Empty)
        {
            StatusCode = statusCode;
        }

        public BaseDomainException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }

        public BaseDomainException(int statusCode, Exception baseExcetion) : base(string.Empty, baseExcetion)
        {
            StatusCode = statusCode;
        }

        public BaseDomainException(string message, int statusCode, Exception baseExcetion) : base(message, baseExcetion)
        {
            StatusCode = statusCode;
        }


        public int StatusCode { get; private set; }
    }
}
