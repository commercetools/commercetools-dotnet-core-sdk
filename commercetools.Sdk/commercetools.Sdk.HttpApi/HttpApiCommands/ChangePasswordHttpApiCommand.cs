using System.Net.Http;
using commercetools.Sdk.Client;
using commercetools.Sdk.HttpApi.RequestBuilders;

namespace commercetools.Sdk.HttpApi.HttpApiCommands
{
    public class ChangePasswordHttpApiCommand<T> : IHttpApiCommandGeneric<ChangePasswordCommand<T>, T>
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
