using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace commercetools.Sdk.HttpApi.Domain
{
    public class NotFoundException : HttpApiClientException
    {
        private const int Code = 404;
        
        public NotFoundException(string message, List<Error> errors) : base(message,Code,errors)
        {
        }
    }
}