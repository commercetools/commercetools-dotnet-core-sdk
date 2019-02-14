namespace commercetools.Sdk.HttpApi.Domain.Exceptions
{
    /// <summary>
    /// An exception has been thrown on the server side.
    /// </summary>
    /// <seealso cref="commercetools.Sdk.HttpApi.Domain.Exceptions.ServerErrorException" />
    public class InternalServerErrorException : ServerErrorException
    {
        public override int StatusCode => 500;
        
        public InternalServerErrorException() 
        {
        }
    }
}