using commercetools.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace commercetools.Sdk.HttpApi
{
    public class GetByIdRequestMessageBuilder : IRequestMessageBuilder
    {
        public static Type CommandType = typeof(GetByIdCommand);

        private GetByIdCommand command;

        public GetByIdRequestMessageBuilder(GetByIdCommand command)
        {
            this.command = command;
        }

        public HttpMethod HttpMethod => HttpMethod.Get;
        public string RequestUriEnd => $"/{this.command.Guid}";
    }
}
