using commercetools.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace commercetools.Sdk.HttpApi
{
    public class DeleteByIdHttpApiCommand<T>: IHttpApiCommand, IRequestable<DeleteByIdCommand<T>>
    {
        private DeleteByIdCommand<T> command;
        private readonly DeleteByIdRequestMessageBuilder requestBuilder;

        public HttpRequestMessage HttpRequestMessage
        {
            get
            {
                return this.requestBuilder.GetRequestMessage(command);
            }
        }

        public DeleteByIdHttpApiCommand(DeleteByIdCommand<T> command, IRequestMessageBuilderFactory requestMessageBuilderFactory)
        {
            this.command = command;
            this.requestBuilder = requestMessageBuilderFactory.GetRequestMessageBuilder<DeleteByIdRequestMessageBuilder>();
        }


    }
}
