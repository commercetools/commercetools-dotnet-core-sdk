using commercetools.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace commercetools.Sdk.HttpApi
{
    // TODO See if the code can be simplified and if common pieces can be combined
    public class CreateHttpApiCommand<T>: IHttpApiCommand, IRequestable<CreateCommand<T>>
    {
        private CreateCommand<T> command;
        private readonly CreateRequestMessageBuilder requestBuilder;

        public HttpRequestMessage HttpRequestMessage
        {
            get
            {
                return this.requestBuilder.GetRequestMessage(command);
            }
        }

        public CreateHttpApiCommand(CreateCommand<T> command, IRequestMessageBuilderFactory requestMessageBuilderFactory)
        {
            this.command = command;
            this.requestBuilder = requestMessageBuilderFactory.GetRequestMessageBuilder<CreateRequestMessageBuilder>();
        }
    }
}
