using System.Collections.Generic;

namespace commercetools.Sdk.HttpApi.Domain.Exceptions
{
    public class NotFoundException : ClientErrorException
    {
        private const int Code = 404;

        public NotFoundException() : base(Code)
        {
        }
    }
}