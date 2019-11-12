using commercetools.Sdk.HttpApi.RequestBuilders;

namespace commercetools.Sdk.HttpApi.HttpApiCommands
{
    using System.Net.Http;
    using commercetools.Sdk.Client;

    public class GetByContainerAndKeyHttpApiCommand<T> : IHttpApiCommandGeneric<GetByContainerAndKeyCommand<T>, T>
    {
        private readonly GetByContainerAndKeyCommand<T> command;
        private readonly GetByContainerAndKeyMessageBuilder requestBuilder;

        public GetByContainerAndKeyHttpApiCommand(GetByContainerAndKeyCommand<T> command, IRequestMessageBuilderFactory requestMessageBuilderFactory)
        {
            this.command = command;
            this.requestBuilder = requestMessageBuilderFactory.GetRequestMessageBuilder<GetByContainerAndKeyMessageBuilder>();
        }

        public HttpRequestMessage HttpRequestMessage => this.requestBuilder.GetRequestMessage(this.command);
    }
}
