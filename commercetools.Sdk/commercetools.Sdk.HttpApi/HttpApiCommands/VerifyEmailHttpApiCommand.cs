using System.Net.Http;
using commercetools.Sdk.Client;
using commercetools.Sdk.HttpApi.RequestBuilders;

namespace commercetools.Sdk.HttpApi.HttpApiCommands
{
    public class VerifyEmailHttpApiCommand<T> : IHttpApiCommandGeneric<VerifyEmailCommand<T>, T>
    {
        private readonly VerifyEmailCommand<T> command;
        private readonly VerifyEmailRequestMessageBuilder requestBuilder;

        public VerifyEmailHttpApiCommand(VerifyEmailCommand<T> command, IRequestMessageBuilderFactory requestMessageBuilderFactory)
        {
            this.command = command;
            this.requestBuilder = requestMessageBuilderFactory.GetRequestMessageBuilder<VerifyEmailRequestMessageBuilder>();
        }

        public HttpRequestMessage HttpRequestMessage => this.requestBuilder.GetRequestMessage(this.command);
    }
}
