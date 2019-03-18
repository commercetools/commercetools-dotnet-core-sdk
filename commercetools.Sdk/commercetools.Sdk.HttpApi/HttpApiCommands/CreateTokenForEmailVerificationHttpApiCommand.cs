using System.Net.Http;
using commercetools.Sdk.Client;
using commercetools.Sdk.HttpApi.RequestBuilders;

namespace commercetools.Sdk.HttpApi.HttpApiCommands
{
    public class CreateTokenForEmailVerificationHttpApiCommand<T> : IHttpApiCommandGeneric<CreateTokenForEmailVerificationCommand<T>, T>
    {
        private readonly CreateTokenForEmailVerificationCommand<T> command;
        private readonly CreateTokenForEmailVerificationRequestMessageBuilder requestBuilder;

        public CreateTokenForEmailVerificationHttpApiCommand(CreateTokenForEmailVerificationCommand<T> command, IRequestMessageBuilderFactory requestMessageBuilderFactory)
        {
            this.command = command;
            this.requestBuilder = requestMessageBuilderFactory.GetRequestMessageBuilder<CreateTokenForEmailVerificationRequestMessageBuilder>();
        }

        public HttpRequestMessage HttpRequestMessage => this.requestBuilder.GetRequestMessage(this.command);
    }
}
