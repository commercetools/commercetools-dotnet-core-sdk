using System.Net.Http;
using commercetools.Sdk.Client;
using commercetools.Sdk.HttpApi.RequestBuilders;

namespace commercetools.Sdk.HttpApi.HttpApiCommands
{
    public class QueryHttpApiCommand<T> : IHttpApiCommand<QueryCommand<T>, T>
    {
        private readonly QueryCommand<T> command;
        private readonly QueryRequestMessageBuilder requestBuilder;

        public QueryHttpApiCommand(QueryCommand<T> command, IRequestMessageBuilderFactory requestMessageBuilderFactory)
        {
            this.command = command;
            this.requestBuilder = requestMessageBuilderFactory.GetRequestMessageBuilder<QueryRequestMessageBuilder>();
        }

        public HttpRequestMessage HttpRequestMessage => this.requestBuilder.GetRequestMessage(this.command);
    }
}