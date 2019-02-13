namespace commercetools.Sdk.HttpApi.Domain.Exceptions
{
    /// <summary>
    /// This error might occur on long running processes such as deletion of resources with connections to other resources.
    /// </summary>
    public class GatewayTimeoutException : ServerErrorException
    {
        public override int StatusCode => 504;

        public GatewayTimeoutException() 
        {
        }
    }
}