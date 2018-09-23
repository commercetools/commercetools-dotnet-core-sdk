using commercetools.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace commercetools.Sdk.HttpApi
{
    public class UpdateByKeyHttpApiCommand<T>: IHttpApiCommand, IRequestable<UpdateByKeyCommand<T>>
    {
        private UpdateByKeyCommand<T> command;
        private readonly UpdateByKeyRequestMessageBuilder requestBuilder;

        public HttpRequestMessage HttpRequestMessage
        {
            get
            {
                return this.requestBuilder.GetRequestMessage(command);
            }
        }

        public UpdateByKeyHttpApiCommand(UpdateByKeyCommand<T> command, IRequestMessageBuilderFactory requestMessageBuilderFactory)
        {
            this.command = command;
            this.requestBuilder = requestMessageBuilderFactory.GetRequestMessageBuilder<UpdateByKeyRequestMessageBuilder>();
        }


    }
}
