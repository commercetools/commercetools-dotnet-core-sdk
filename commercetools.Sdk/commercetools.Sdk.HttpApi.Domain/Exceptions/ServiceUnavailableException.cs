namespace commercetools.Sdk.HttpApi.Domain.Exceptions
{
    /// <summary>
    /// The platform is currently not available.
    /// </summary>
    public class ServiceUnavailableException : ServerErrorException
    {
        public override int StatusCode => 503;

        public ServiceUnavailableException() 
        {
        }
    }
}