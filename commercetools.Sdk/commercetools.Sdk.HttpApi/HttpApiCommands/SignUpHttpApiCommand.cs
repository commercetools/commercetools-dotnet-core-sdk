using System.Net.Http;
using commercetools.Sdk.Client;
using commercetools.Sdk.HttpApi.RequestBuilders;

namespace commercetools.Sdk.HttpApi.HttpApiCommands
{
    public class SignUpHttpApiCommand<T> : IHttpApiCommand<SignUpCommand<T>, T>
    {
        private readonly SignUpCommand<T> command;
        private readonly SignUpRequestMessageBuilder requestBuilder;

        public SignUpHttpApiCommand(SignUpCommand<T> command, IRequestMessageBuilderFactory requestMessageBuilderFactory)
        {
            this.command = command;
            this.requestBuilder = requestMessageBuilderFactory.GetRequestMessageBuilder<SignUpRequestMessageBuilder>();
        }

        public HttpRequestMessage HttpRequestMessage => this.requestBuilder.GetRequestMessage(this.command);
    }
}