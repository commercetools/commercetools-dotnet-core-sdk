using System.Net.Http;
using commercetools.Sdk.Client;
using commercetools.Sdk.HttpApi.RequestBuilders;

namespace commercetools.Sdk.HttpApi.HttpApiCommands
{
    public class GetMatchingQueryHttpApiCommand<T> : IHttpApiCommandGeneric<GetMatchingQueryCommand<T>, T>
    {
        private readonly GetMatchingQueryCommand<T> command;
        private readonly GetMatchingQueryRequestMessageBuilder requestBuilder;

        public GetMatchingQueryHttpApiCommand(GetMatchingQueryCommand<T> command, IRequestMessageBuilderFactory requestMessageBuilderFactory)
        {
            this.command = command;
            this.requestBuilder = requestMessageBuilderFactory.GetRequestMessageBuilder<GetMatchingQueryRequestMessageBuilder>();
        }

        public HttpRequestMessage HttpRequestMessage => this.requestBuilder.GetRequestMessage(this.command);
    }
}
