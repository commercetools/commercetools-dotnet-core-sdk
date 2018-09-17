using commercetools.Sdk.Client;
using commercetools.Sdk.Serialization;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace commercetools.Sdk.HttpApi
{
    public class CreateRequestMessageBuilder : IRequestMessageBuilder
    {
        public static Type CommandType = typeof(CreateCommand);
        private CreateCommand command;

        public CreateRequestMessageBuilder(CreateCommand command)
        {
            this.command = command;
        }

        public object RequestBody => command.Entity;

        public HttpMethod HttpMethod => HttpMethod.Post;

        public string RequestUriEnd => $"/";
    }
}
