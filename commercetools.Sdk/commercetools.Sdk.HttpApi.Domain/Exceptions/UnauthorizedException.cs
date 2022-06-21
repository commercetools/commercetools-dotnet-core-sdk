using System.Runtime.CompilerServices;

namespace commercetools.Sdk.HttpApi.Domain.Exceptions
{
    /// <summary>
    /// Unauthorized access to Composable Commerce with either invalid client credentials or tokens.
    /// Most likely the subclass exceptions InvalidTokenException will be thrown.
    /// </summary>
    /// <seealso cref="commercetools.Sdk.HttpApi.Domain.Exceptions.InvalidTokenException" />
    public class UnauthorizedException : ClientErrorException
    {
        public override int StatusCode => 401;

        public UnauthorizedException()
        {

        }
    }
}