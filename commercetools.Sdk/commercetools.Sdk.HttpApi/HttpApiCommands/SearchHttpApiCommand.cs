using commercetools.Sdk.HttpApi.RequestBuilders;

namespace commercetools.Sdk.HttpApi
{
    using System.Net.Http;
    using commercetools.Sdk.Client;

    public class SearchHttpApiCommand<T> : IHttpApiCommandGeneric<SearchCommand<T>, T>
    {
        private readonly SearchCommand<T> command;
        private readonly SearchRequestMessageBuilder requestBuilder;

        public SearchHttpApiCommand(SearchCommand<T> command, IRequestMessageBuilderFactory requestMessageBuilderFactory)
        {
            this.command = command;
            this.requestBuilder = requestMessageBuilderFactory.GetRequestMessageBuilder<SearchRequestMessageBuilder>();
        }

        public HttpRequestMessage HttpRequestMessage => this.requestBuilder.GetRequestMessage(this.command);
    }
}
