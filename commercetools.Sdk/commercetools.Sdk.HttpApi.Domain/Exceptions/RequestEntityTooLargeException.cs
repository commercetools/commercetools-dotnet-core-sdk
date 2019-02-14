namespace commercetools.Sdk.HttpApi.Domain.Exceptions
{
    /// <summary>
    /// HTTP code 413 response from the platform.
    /// Probable error cause: Predicate for a query is too long. Try to split the request into multiple ones or use the in operator.
    /// </summary>
    public class RequestEntityTooLargeException : ClientErrorException
    {
        public override int StatusCode => 413;

        public RequestEntityTooLargeException() 
        {
        }
    }
}