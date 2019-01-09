using commercetools.Sdk.Client;
using System.Net.Http;
using commercetools.Sdk.HttpApi.RequestBuilders;

namespace commercetools.Sdk.HttpApi.HttpApiCommands
{
    public class ChangePasswordHttpApiCommand<T> : IHttpApiCommand<ChangePasswordCommand<T>, T>
    {
        private readonly ChangePasswordCommand<T> command;
        private readonly ChangePasswordRequestMessageBuilder requestBuilder;

        public ChangePasswordHttpApiCommand(ChangePasswordCommand<T> command, IRequestMessageBuilderFactory requestMessageBuilderFactory)
        {
            this.command = command;
            this.requestBuilder = requestMessageBuilderFactory.GetRequestMessageBuilder<ChangePasswordRequestMessageBuilder>();
        }

        public HttpRequestMessage HttpRequestMessage => this.requestBuilder.GetRequestMessage(this.command);
    }
}