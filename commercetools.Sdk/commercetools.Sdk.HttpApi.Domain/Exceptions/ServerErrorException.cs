namespace commercetools.Sdk.HttpApi.Domain.Exceptions
{
    /// <summary>
    /// Exceptions with status code (>= 500)
    /// </summary>
    public abstract class ServerErrorException : ApiServiceException
    {
        public override int StatusCode { get; set; }
    }
}