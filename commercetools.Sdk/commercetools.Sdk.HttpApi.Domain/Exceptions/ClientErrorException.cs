namespace commercetools.Sdk.HttpApi.Domain.Exceptions
{
    public class ClientErrorException : ApiServiceException
    {
        public ClientErrorException(int statusCode) : base(statusCode)
        {
        }

        public ClientErrorException(int statusCode, string message) : base(statusCode, message)
        {
        }
    }
}