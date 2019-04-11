using System.Net.Http;
using commercetools.Sdk.Client;
using commercetools.Sdk.HttpApi.RequestBuilders;

namespace commercetools.Sdk.HttpApi.HttpApiCommands
{
    public class GraphQLHttpApiCommand<T> : IHttpApiCommandGeneric<GraphQLCommand<T>, T>
    {
        private readonly GraphQLCommand<T> command;
        private readonly GraphQLRequestMessageBuilder requestBuilder;

        public GraphQLHttpApiCommand(GraphQLCommand<T> command, IRequestMessageBuilderFactory requestMessageBuilderFactory)
        {
            this.command = command;
            this.requestBuilder = requestMessageBuilderFactory.GetRequestMessageBuilder<GraphQLRequestMessageBuilder>();
        }

        public HttpRequestMessage HttpRequestMessage => this.requestBuilder.GetRequestMessage(this.command);
    }
}
