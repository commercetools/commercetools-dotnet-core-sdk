using System.Net.Http;
using commercetools.Sdk.Client;
using commercetools.Sdk.HttpApi.RequestBuilders;

namespace commercetools.Sdk.HttpApi.HttpApiCommands
{
    public class QueryByContainerHttpApiCommand<T> : IHttpApiCommandGeneric<QueryByContainerCommand<T>, T>
    {
        private readonly QueryByContainerCommand<T> command;
        private readonly QueryByContainerRequestMessageBuilder requestBuilder;

        public QueryByContainerHttpApiCommand(QueryByContainerCommand<T> command, IRequestMessageBuilderFactory requestMessageBuilderFactory)
        {
            this.command = command;
            this.requestBuilder = requestMessageBuilderFactory.GetRequestMessageBuilder<QueryByContainerRequestMessageBuilder>();
        }

        public HttpRequestMessage HttpRequestMessage => this.requestBuilder.GetRequestMessage(this.command);
    }
}
