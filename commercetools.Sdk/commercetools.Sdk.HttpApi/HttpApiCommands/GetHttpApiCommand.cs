using commercetools.Sdk.HttpApi.RequestBuilders;

namespace commercetools.Sdk.HttpApi.HttpApiCommands
{
    using System.Net.Http;
    using commercetools.Sdk.Client;

    public class GetHttpApiCommand<T> : IHttpApiCommandGeneric<GetCommand<T>, T>
    {
        private readonly GetCommand<T> command;
        private readonly GetRequestMessageBuilder requestBuilder;

        public GetHttpApiCommand(GetCommand<T> command, IRequestMessageBuilderFactory requestMessageBuilderFactory)
        {
            this.command = command;
            this.requestBuilder = requestMessageBuilderFactory.GetRequestMessageBuilder<GetRequestMessageBuilder>();
        }

        public HttpRequestMessage HttpRequestMessage => this.requestBuilder.GetRequestMessage(this.command);
    }
}
