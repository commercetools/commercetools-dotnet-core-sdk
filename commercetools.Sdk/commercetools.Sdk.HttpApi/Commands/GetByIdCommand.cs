using commercetools.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace commercetools.Sdk.HttpApi
{
    public class GetByIdCommand : IHttpApiCommand
    {
        private Guid guid;

        public GetByIdCommand(Guid guid)
        {
            this.guid = guid;
        }

        public HttpMethod HttpMethod => HttpMethod.Get;

        public string RequestUriEnd => $"/{this.guid}";
    }
}
