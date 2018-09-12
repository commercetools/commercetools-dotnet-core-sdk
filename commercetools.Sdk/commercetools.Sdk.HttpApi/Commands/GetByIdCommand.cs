using System;
using System.Net.Http;

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