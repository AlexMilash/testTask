namespace TestTask.PostingsClient.Contracts.Exceptions
{
    public class UnexpectedExternalDataFormatException : BaseDomainException
    {
        public UnexpectedExternalDataFormatException(int statusCode) : base(statusCode)
        {
        }

        public UnexpectedExternalDataFormatException(string message, int statusCode) : base(message, statusCode)
        {
        }

        public UnexpectedExternalDataFormatException(int statusCode, Exception baseExcetion) : base(statusCode, baseExcetion)
        {
        }

        public UnexpectedExternalDataFormatException(string message, int statusCode, Exception baseExcetion) : base(message, statusCode, baseExcetion)
        {
        }
    }
}
