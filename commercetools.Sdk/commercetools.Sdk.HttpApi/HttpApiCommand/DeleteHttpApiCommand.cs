namespace commercetools.Sdk.HttpApi
{
    using System.Net.Http;
    using commercetools.Sdk.Client;

    public class DeleteHttpApiCommand<T> : IHttpApiCommand<DeleteCommand<T>, T>
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