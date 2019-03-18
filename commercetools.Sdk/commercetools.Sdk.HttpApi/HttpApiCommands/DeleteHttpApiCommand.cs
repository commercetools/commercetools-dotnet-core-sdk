using commercetools.Sdk.HttpApi.RequestBuilders;

namespace commercetools.Sdk.HttpApi.HttpApiCommands
{
    using System.Net.Http;
    using commercetools.Sdk.Client;

    public class DeleteHttpApiCommand<T> : IHttpApiCommandGeneric<DeleteCommand<T>, T>
    {
        private readonly DeleteCommand<T> command;
        private readonly DeleteRequestMessageBuilder requestBuilder;

        public DeleteHttpApiCommand(DeleteCommand<T> command, IRequestMessageBuilderFactory requestMessageBuilderFactory)
        {
            this.command = command;
            this.requestBuilder = requestMessageBuilderFactory.GetRequestMessageBuilder<DeleteRequestMessageBuilder>();
        }

        public HttpRequestMessage HttpRequestMessage => this.requestBuilder.GetRequestMessage(this.command);
    }
}
