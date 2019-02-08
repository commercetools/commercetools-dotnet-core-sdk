namespace commercetools.Sdk.HttpApi.Domain.Exceptions
{
    public class BadRequestException : ClientErrorException
    {
        private const int Code = 400;

        public BadRequestException(string message) : base(Code, message)
        {
        }
    }
}