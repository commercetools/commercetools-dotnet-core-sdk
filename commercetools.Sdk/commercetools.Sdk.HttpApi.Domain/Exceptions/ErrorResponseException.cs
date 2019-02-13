namespace commercetools.Sdk.HttpApi.Domain.Exceptions
{
    ///  <summary>
    ///  Typical exception for bad requests containing error information.
    ///  </summary>
    /// <seealso>
    ///     <cref>https://docs.commercetools.com/http-api-errors#400-bad-request</cref>
    /// <remarks>list of error codes can be found on the commercetools website.</remarks>
    /// </seealso>
    public class ErrorResponseException : BadRequestException
    {
        public override int StatusCode => 400;

        public ErrorResponseException() 
        {
        }
    }
}