using commercetools.Sdk.Client;
using System.Net.Http;

namespace commercetools.Sdk.HttpApi
{
    public class DeleteHttpApiCommand<T> : IHttpApiCommand<DeleteCommand<T>, T>
    {
        private DeleteCommand<T> command;
        private readonly DeleteRequestMessageBuilder requestBuilder;

        public HttpRequestMessage HttpRequestMessage
        {
            get
            {
                return this.requestBuilder.GetRequestMessage(command);
            }
        }

        public DeleteHttpApiCommand(DeleteCommand<T> command, IRequestMessageBuilderFactory requestMessageBuilderFactory)
        {
            this.command = command;
            this.requestBuilder = requestMessageBuilderFactory.GetRequestMessageBuilder<DeleteRequestMessageBuilder>();
        }
    }
}