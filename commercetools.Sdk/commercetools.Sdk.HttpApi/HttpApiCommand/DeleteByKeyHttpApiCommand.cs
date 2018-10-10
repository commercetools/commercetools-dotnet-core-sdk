using commercetools.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace commercetools.Sdk.HttpApi
{
    public class DeleteByKeyHttpApiCommand<T>: IHttpApiCommand<DeleteByKeyCommand<T>, T> //, IRequestable<DeleteByKeyCommand<T>>
    {
        private DeleteByKeyCommand<T> command;
        private readonly DeleteByKeyRequestMessageBuilder requestBuilder;

        public HttpRequestMessage HttpRequestMessage
        {
            get
            {
                return this.requestBuilder.GetRequestMessage(command);
            }
        }

        public DeleteByKeyHttpApiCommand(DeleteByKeyCommand<T> command, IRequestMessageBuilderFactory requestMessageBuilderFactory)
        {
            this.command = command;
            this.requestBuilder = requestMessageBuilderFactory.GetRequestMessageBuilder<DeleteByKeyRequestMessageBuilder>();
        }


    }
}
