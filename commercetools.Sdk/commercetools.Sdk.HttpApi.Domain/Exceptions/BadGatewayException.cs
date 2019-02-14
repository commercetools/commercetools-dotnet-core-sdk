namespace commercetools.Sdk.HttpApi.Domain.Exceptions
{
    /// <summary>
    /// On the server occurred a problem, try again later.
    /// </summary>
    public class BadGatewayException : ServerErrorException
    {
        public override int StatusCode => 502;

        public BadGatewayException() 
        {
        }
    }
}