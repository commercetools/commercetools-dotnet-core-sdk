using System.Net.Http;
using commercetools.Sdk.Client;
using commercetools.Sdk.HttpApi.RequestBuilders;

namespace commercetools.Sdk.HttpApi.HttpApiCommands
{
    public class UpsertHttpApiCommand<T> : IHttpApiCommandGeneric<UpsertCommand<T>, T>
    {
        private readonly UpsertCommand<T> command;
        private readonly UpsertRequestMessageBuilder requestBuilder;

        public UpsertHttpApiCommand(UpsertCommand<T> command, IRequestMessageBuilderFactory requestMessageBuilderFactory)
        {
            this.command = command;
            this.requestBuilder = requestMessageBuilderFactory.GetRequestMessageBuilder<UpsertRequestMessageBuilder>();
        }

        public HttpRequestMessage HttpRequestMessage => this.requestBuilder.GetRequestMessage(this.command);
    }
}
