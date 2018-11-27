namespace commercetools.Sdk.HttpApi
{
    using commercetools.Sdk.Client;
    using System.Net.Http;

    public class SearchHttpApiCommand<T> : IHttpApiCommand<SearchCommand<T>, T>
    {
        private SearchCommand<T> command;
        private readonly SearchRequestMessageBuilder requestBuilder;

        public HttpRequestMessage HttpRequestMessage
        {
            get
            {
                return this.requestBuilder.GetRequestMessage(command);
            }
        }

        public SearchHttpApiCommand(SearchCommand<T> command, IRequestMessageBuilderFactory requestMessageBuilderFactory)
        {
            this.command = command;
            this.requestBuilder = requestMessageBuilderFactory.GetRequestMessageBuilder<SearchRequestMessageBuilder>();
        }
    }
}