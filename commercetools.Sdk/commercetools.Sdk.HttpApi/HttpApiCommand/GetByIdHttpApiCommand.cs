using commercetools.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace commercetools.Sdk.HttpApi
{
    public class GetByIdHttpApiCommand<T>: IHttpApiCommand, IRequestable<GetByIdCommand<T>>
    {
        private GetByIdCommand<T> command;
        private readonly GetByIdRequestMessageBuilder requestBuilder;

        public HttpRequestMessage HttpRequestMessage
        {
            get
            {
                return this.requestBuilder.GetRequestMessage(command);
            }
        }

        public GetByIdHttpApiCommand(GetByIdCommand<T> command, IRequestMessageBuilderFactory requestMessageBuilderFactory)
        {
            this.command = command;
            this.requestBuilder = requestMessageBuilderFactory.GetRequestMessageBuilder<GetByIdRequestMessageBuilder>();
        }


    }
}
