using commercetools.Sdk.Client;
using System.Net.Http;
using commercetools.Sdk.HttpApi.RequestBuilders;

namespace commercetools.Sdk.HttpApi.HttpApiCommands
{
    public class CreateHttpApiCommand<T> : IHttpApiCommand<CreateCommand<T>, T>
    {
        private readonly CreateCommand<T> command;
        private readonly CreateRequestMessageBuilder requestBuilder;

        public CreateHttpApiCommand(CreateCommand<T> command, IRequestMessageBuilderFactory requestMessageBuilderFactory)
        {
            this.command = command;
            this.requestBuilder = requestMessageBuilderFactory.GetRequestMessageBuilder<CreateRequestMessageBuilder>();
        }

        public HttpRequestMessage HttpRequestMessage => this.requestBuilder.GetRequestMessage(this.command);
    }
}