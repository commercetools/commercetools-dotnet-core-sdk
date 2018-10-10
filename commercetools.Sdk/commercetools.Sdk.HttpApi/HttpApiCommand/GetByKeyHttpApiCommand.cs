using commercetools.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace commercetools.Sdk.HttpApi
{
    public class GetByKeyHttpApiCommand<T>: IHttpApiCommand<GetByKeyCommand<T>, T> //, IRequestable<GetByKeyCommand<T>>
    {
        private GetByKeyCommand<T> command;
        private readonly GetByKeyRequestMessageBuilder requestBuilder;

        public HttpRequestMessage HttpRequestMessage
        {
            get
            {
                return this.requestBuilder.GetRequestMessage(command);
            }
        }

        public GetByKeyHttpApiCommand(GetByKeyCommand<T> command, IRequestMessageBuilderFactory requestMessageBuilderFactory)
        {
            this.command = command;
            this.requestBuilder = requestMessageBuilderFactory.GetRequestMessageBuilder<GetByKeyRequestMessageBuilder>();
        }


    }
}
