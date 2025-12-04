namespace TestTask.PostingsClient.Contracts.Exceptions
{
    public class FailedToRetrieveFromExternalSystemException : BaseDomainException
    {
        public FailedToRetrieveFromExternalSystemException(int statusCode) : base(statusCode)
        {
        }

        public FailedToRetrieveFromExternalSystemException(string message, int statusCode) : base(message, statusCode)
        {
        }

        public FailedToRetrieveFromExternalSystemException(int statusCode, Exception baseExcetion) : base(statusCode, baseExcetion)
        {
        }

        public FailedToRetrieveFromExternalSystemException(string message, int statusCode, Exception baseExcetion) : base(message, statusCode, baseExcetion)
        {
        }
    }
}
