using commercetools.Sdk.HttpApi.RequestBuilders;

namespace commercetools.Sdk.HttpApi.HttpApiCommands
{
    using System.Net.Http;
    using commercetools.Sdk.Client;

    public class DeleteByContainerAndKeyHttpApiCommand<T> : IHttpApiCommandGeneric<DeleteByContainerAndKeyCommand<T>, T>
    {
        private readonly DeleteByContainerAndKeyCommand<T> command;
        private readonly DeleteByContainerAndKeyMessageBuilder requestBuilder;

        public DeleteByContainerAndKeyHttpApiCommand(DeleteByContainerAndKeyCommand<T> command, IRequestMessageBuilderFactory requestMessageBuilderFactory)
        {
            this.command = command;
            this.requestBuilder = requestMessageBuilderFactory.GetRequestMessageBuilder<DeleteByContainerAndKeyMessageBuilder>();
        }

        public HttpRequestMessage HttpRequestMessage => this.requestBuilder.GetRequestMessage(this.command);
    }
}
