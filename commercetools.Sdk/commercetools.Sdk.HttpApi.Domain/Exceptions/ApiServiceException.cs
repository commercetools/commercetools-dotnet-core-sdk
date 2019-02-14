namespace commercetools.Sdk.HttpApi.Domain.Exceptions
{
    /// <summary>
    /// Exceptions with status code (other than HTTP 2xx.)
    /// </summary>
    public abstract class ApiServiceException : ApiException
    {
        public abstract int StatusCode { get; set; }

    }
}