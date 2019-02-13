using System.Collections.Generic;

namespace commercetools.Sdk.HttpApi.Domain.Exceptions
{
    /// <summary>
    /// Resource not found
    /// </summary>
    public class NotFoundException : ClientErrorException
    {
        public override int StatusCode => 404;

        public NotFoundException()
        {
        }
    }
}