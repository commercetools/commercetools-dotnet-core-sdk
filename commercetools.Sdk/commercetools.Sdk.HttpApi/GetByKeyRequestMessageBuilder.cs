using commercetools.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace commercetools.Sdk.HttpApi
{
    public class GetByKeyRequestMessageBuilder : IRequestMessageBuilder
    {
        public static Type CommandType = typeof(GetByKeyCommand);

        private GetByKeyCommand command;

        public GetByKeyRequestMessageBuilder(GetByKeyCommand command)
        {
            this.command = command;
        }

        public HttpMethod HttpMethod => HttpMethod.Get;

        public string RequestUriEnd => $"/key={this.command.Key}";
    }
}
