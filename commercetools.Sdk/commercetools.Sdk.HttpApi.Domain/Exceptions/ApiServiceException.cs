namespace commercetools.Sdk.HttpApi.Domain.Exceptions
{
    public abstract class ApiServiceException : ApiException
    {
        public int StatusCode { get; set; }

        public ApiServiceException(int statusCode)
        {
            this.StatusCode = statusCode;
        }
        public ApiServiceException(int statusCode, string message) :base(message)
        {
            this.StatusCode = statusCode;
        }
    }
}