using System.Net.Http;
using commercetools.Sdk.Client;
using commercetools.Sdk.HttpApi.RequestBuilders;

namespace commercetools.Sdk.HttpApi.HttpApiCommands
{
    public class ResetPasswordHttpApiCommand<T> : IHttpApiCommandGeneric<ResetPasswordCommand<T>, T>
    {
        private readonly ResetPasswordCommand<T> command;
        private readonly ResetPasswordRequestMessageBuilder requestBuilder;

        public ResetPasswordHttpApiCommand(ResetPasswordCommand<T> command, IRequestMessageBuilderFactory requestMessageBuilderFactory)
        {
            this.command = command;
            this.requestBuilder = requestMessageBuilderFactory.GetRequestMessageBuilder<ResetPasswordRequestMessageBuilder>();
        }

        public HttpRequestMessage HttpRequestMessage => this.requestBuilder.GetRequestMessage(this.command);
    }
}
