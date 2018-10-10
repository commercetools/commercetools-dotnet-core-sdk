using commercetools.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace commercetools.Sdk.HttpApi
{
    public class QueryHttpApiCommand<T>: IHttpApiCommand<QueryCommand<T>,T> //, IRequestable<QueryCommand<T>>
    {
        private QueryCommand<T> command;
        private readonly QueryRequestMessageBuilder requestBuilder;

        public HttpRequestMessage HttpRequestMessage
        {
            get
            {
                return this.requestBuilder.GetRequestMessage(command);
            }
        }

        public QueryHttpApiCommand(QueryCommand<T> command, IRequestMessageBuilderFactory requestMessageBuilderFactory)
        {
            this.command = command;
            this.requestBuilder = requestMessageBuilderFactory.GetRequestMessageBuilder<QueryRequestMessageBuilder>();
        }


    }
}
