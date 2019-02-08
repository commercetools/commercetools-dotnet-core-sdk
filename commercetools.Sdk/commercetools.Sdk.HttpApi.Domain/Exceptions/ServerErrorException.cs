namespace commercetools.Sdk.HttpApi.Domain.Exceptions
{
    public abstract class ServerErrorException : ApiServiceException
    {
        protected ServerErrorException(int statusCode) : base(statusCode)
        {
        }

        protected ServerErrorException(int statusCode, string message) : base(statusCode, message)
        {
        }
    }
}