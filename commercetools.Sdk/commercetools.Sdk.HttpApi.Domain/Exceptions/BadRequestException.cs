namespace commercetools.Sdk.HttpApi.Domain.Exceptions
{
    /// <summary>
    /// request failed due to providing bad input. Bad input can be a malformed request body, missing required parameters, wrongly typed or malformed parameters or a parameter that references another resource that does not exist.
    /// </summary>
    public class BadRequestException : ClientErrorException
    {
        public override int StatusCode => 400;

        public BadRequestException() 
        {
        }
    }
}