using commercetools.Sdk.Client;
using System.Net.Http;
using commercetools.Sdk.HttpApi.RequestBuilders;

namespace commercetools.Sdk.HttpApi.HttpApiCommands
{
    public class LoginHttpApiCommand<T> : IHttpApiCommand<LoginCommand<T>, T>
    {
        private readonly LoginCommand<T> command;
        private readonly LoginRequestMessageBuilder requestBuilder;

        public LoginHttpApiCommand(LoginCommand<T> command, IRequestMessageBuilderFactory requestMessageBuilderFactory)
        {
            this.command = command;
            this.requestBuilder = requestMessageBuilderFactory.GetRequestMessageBuilder<LoginRequestMessageBuilder>();
        }

        public HttpRequestMessage HttpRequestMessage => this.requestBuilder.GetRequestMessage(this.command);
    }
}