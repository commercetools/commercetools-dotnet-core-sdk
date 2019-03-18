using System.Net.Http;
using commercetools.Sdk.Client;
using commercetools.Sdk.HttpApi.RequestBuilders;

namespace commercetools.Sdk.HttpApi.HttpApiCommands
{
    public class UpdateHttpApiCommand<T> : IHttpApiCommandGeneric<UpdateCommand<T>, T>
    {
        private readonly UpdateCommand<T> command;
        private readonly UpdateRequestMessageBuilder requestBuilder;

        public UpdateHttpApiCommand(UpdateCommand<T> command, IRequestMessageBuilderFactory requestMessageBuilderFactory)
        {
            this.command = command;
            this.requestBuilder = requestMessageBuilderFactory.GetRequestMessageBuilder<UpdateRequestMessageBuilder>();
        }

        public HttpRequestMessage HttpRequestMessage => this.requestBuilder.GetRequestMessage(this.command);
    }
}
