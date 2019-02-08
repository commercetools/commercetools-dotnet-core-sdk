namespace commercetools.Sdk.HttpApi.Domain.Exceptions
{
    public class UnauthorizedException : ClientErrorException
    {
        private const int Code = 401;
        
        public UnauthorizedException(string message) : base(Code, message)
        {
            
        }
    }
}