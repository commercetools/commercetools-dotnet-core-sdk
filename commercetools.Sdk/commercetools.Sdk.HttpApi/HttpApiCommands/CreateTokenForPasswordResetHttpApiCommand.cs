using System.Net.Http;
using commercetools.Sdk.Client;
using commercetools.Sdk.HttpApi.RequestBuilders;

namespace commercetools.Sdk.HttpApi.HttpApiCommands
{
    public class CreateTokenForPasswordResetHttpApiCommand<T> : IHttpApiCommandGeneric<CreateTokenForPasswordResetCommand<T>, T>
    {
        private readonly CreateTokenForPasswordResetCommand<T> command;
        private readonly CreateTokenForPasswordResetRequestMessageBuilder requestBuilder;

        public CreateTokenForPasswordResetHttpApiCommand(CreateTokenForPasswordResetCommand<T> command, IRequestMessageBuilderFactory requestMessageBuilderFactory)
        {
            this.command = command;
            this.requestBuilder = requestMessageBuilderFactory.GetRequestMessageBuilder<CreateTokenForPasswordResetRequestMessageBuilder>();
        }

        public HttpRequestMessage HttpRequestMessage => this.requestBuilder.GetRequestMessage(this.command);
    }
}
