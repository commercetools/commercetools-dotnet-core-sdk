using System.Net.Http;
using commercetools.Sdk.Client;
using commercetools.Sdk.HttpApi.RequestBuilders;

namespace commercetools.Sdk.HttpApi.HttpApiCommands
{
    public class ImportHttpApiCommand<T> : IHttpApiCommandGeneric<ImportCommand<T>, T>
    {
        private readonly ImportCommand<T> command;
        private readonly ImportRequestMessageBuilder requestBuilder;

        public ImportHttpApiCommand(ImportCommand<T> command, IRequestMessageBuilderFactory requestMessageBuilderFactory)
        {
            this.command = command;
            this.requestBuilder = requestMessageBuilderFactory.GetRequestMessageBuilder<ImportRequestMessageBuilder>();
        }

        public HttpRequestMessage HttpRequestMessage => this.requestBuilder.GetRequestMessage(this.command);
    }
}
