using commercetools.Sdk.HttpApi.RequestBuilders;

namespace commercetools.Sdk.HttpApi.HttpApiCommands
{
    using System.Net.Http;
    using commercetools.Sdk.Client;

    public class GetMatchingHttpApiCommand<T> : IHttpApiCommandGeneric<GetMatchingCommand<T>, T>
    {
        private readonly GetMatchingCommand<T> command;
        private readonly GetMatchingRequestMessageBuilder requestBuilder;

        public GetMatchingHttpApiCommand(GetMatchingCommand<T> command, IRequestMessageBuilderFactory requestMessageBuilderFactory)
        {
            this.command = command;
            this.requestBuilder = requestMessageBuilderFactory.GetRequestMessageBuilder<GetMatchingRequestMessageBuilder>();
        }

        public HttpRequestMessage HttpRequestMessage => this.requestBuilder.GetRequestMessage(this.command);
    }
}
