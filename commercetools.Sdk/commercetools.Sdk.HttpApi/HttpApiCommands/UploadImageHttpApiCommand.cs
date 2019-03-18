using System.Net.Http;
using commercetools.Sdk.Client;
using commercetools.Sdk.HttpApi.RequestBuilders;

namespace commercetools.Sdk.HttpApi.HttpApiCommands
{
    public class UploadImageHttpApiCommand<T> : IHttpApiCommandGeneric<UploadImageCommand<T>, T>
    {
        private readonly UploadImageCommand<T> command;
        private readonly UploadImageRequestMessageBuilder requestBuilder;

        public UploadImageHttpApiCommand(UploadImageCommand<T> command, IRequestMessageBuilderFactory requestMessageBuilderFactory)
        {
            this.command = command;
            this.requestBuilder = requestMessageBuilderFactory.GetRequestMessageBuilder<UploadImageRequestMessageBuilder>();
        }

        public HttpRequestMessage HttpRequestMessage => this.requestBuilder.GetRequestMessage(this.command);
    }
}
