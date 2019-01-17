using commercetools.Sdk.Client;
using System.Net.Http;
using commercetools.Sdk.HttpApi.RequestBuilders;

namespace commercetools.Sdk.HttpApi.HttpApiCommands
{
    public class CreateTokenForPasswordResetHttpApiCommand<T> : IHttpApiCommand<CreateTokenForPasswordResetCommand<T>, T>
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