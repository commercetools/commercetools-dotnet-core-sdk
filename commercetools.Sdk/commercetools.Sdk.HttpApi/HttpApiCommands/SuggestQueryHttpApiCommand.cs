using System.Net.Http;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain.Suggestions;
using commercetools.Sdk.HttpApi.RequestBuilders;

namespace commercetools.Sdk.HttpApi.HttpApiCommands
{
    public class SuggestQueryHttpApiCommand<T> : IHttpApiCommandGeneric<SuggestQueryCommand<T>, T>
        where T : ISuggestion
    {
        private readonly SuggestQueryCommand<T> command;
        private readonly SuggestQueryRequestMessageBuilder requestBuilder;

        public SuggestQueryHttpApiCommand(SuggestQueryCommand<T> command, IRequestMessageBuilderFactory requestMessageBuilderFactory)
        {
            this.command = command;
            this.requestBuilder = requestMessageBuilderFactory.GetRequestMessageBuilder<SuggestQueryRequestMessageBuilder>();
        }

        public HttpRequestMessage HttpRequestMessage => this.requestBuilder.GetRequestMessage(this.command);
    }
}
