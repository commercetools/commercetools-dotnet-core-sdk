using System;
using System.Net.Http;

namespace commercetools.Sdk.Client
{
    public class GetByIdCommand<T> : GetCommand<T>
    {      
        public GetByIdCommand(Guid guid)
        {
            this.ParameterKey = "id";
            this.ParameterValue = guid;
        }
    }
}