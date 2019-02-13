namespace commercetools.Sdk.HttpApi.Domain.Exceptions
{
    /// <summary>
    /// The commercetools platform received the request but refuses to process it, typically to insufficient rights.
    /// For example it happens if an access token has been received to view products but a request to the customers endpoint is requested.
    /// </summary>
    /// <seealso cref="commercetools.Sdk.HttpApi.Domain.Exceptions.ClientErrorException" />
    public class ForbiddenException : ClientErrorException
    {
        public override int StatusCode => 403;
        
        public ForbiddenException() 
        {
        }
    }
}