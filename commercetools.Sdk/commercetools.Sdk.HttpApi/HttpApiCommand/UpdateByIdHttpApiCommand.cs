using commercetools.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace commercetools.Sdk.HttpApi
{
    public class UpdateByIdHttpApiCommand<T>: IHttpApiCommand, IRequestable<UpdateByIdCommand<T>>
    {
        private UpdateByIdCommand<T> command;
        private readonly UpdateByIdRequestMessageBuilder requestBuilder;

        public HttpRequestMessage HttpRequestMessage
        {
            get
            {
                return this.requestBuilder.GetRequestMessage(command);
            }
        }

        public UpdateByIdHttpApiCommand(UpdateByIdCommand<T> command, IRequestMessageBuilderFactory requestMessageBuilderFactory)
        {
            this.command = command;
            this.requestBuilder = requestMessageBuilderFactory.GetRequestMessageBuilder<UpdateByIdRequestMessageBuilder>();
        }


    }
}
