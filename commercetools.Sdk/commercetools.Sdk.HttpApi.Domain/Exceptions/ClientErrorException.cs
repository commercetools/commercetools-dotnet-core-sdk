namespace commercetools.Sdk.HttpApi.Domain.Exceptions
{
    /// <summary>
    /// Exceptions with status code (4xx)
    /// </summary>
    public class ClientErrorException : ApiServiceException
    {
        public override int StatusCode { get; set; }
        
        public ClientErrorException()
        {
            
        }
    }
}