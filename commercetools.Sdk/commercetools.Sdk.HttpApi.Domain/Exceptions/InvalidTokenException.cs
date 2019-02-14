namespace commercetools.Sdk.HttpApi.Domain.Exceptions
{
    /// <summary>
    /// Invalid token for request (401)
    /// Exception raised in case the access token is not valid for the used commercetools platform project.
    /// </summary>
    /// <seealso cref="commercetools.Sdk.HttpApi.Domain.Exceptions.UnauthorizedException" />
    public class InvalidTokenException : UnauthorizedException
    {
        public override int StatusCode => 401;
        
        public InvalidTokenException() 
        {
        }
    }
}