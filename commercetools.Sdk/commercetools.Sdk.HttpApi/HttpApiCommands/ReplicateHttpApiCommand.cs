using commercetools.Sdk.HttpApi.RequestBuilders;

namespace commercetools.Sdk.HttpApi.HttpApiCommands
{
    using System.Net.Http;
    using commercetools.Sdk.Client;

    public class ReplicateHttpApiCommand<T> : IHttpApiCommandGeneric<ReplicateCommand<T>, T>
    {
        private readonly ReplicateCommand<T> command;
        private readonly ReplicateRequestMessageBuilder requestBuilder;

        public ReplicateHttpApiCommand(ReplicateCommand<T> command, IRequestMessageBuilderFactory requestMessageBuilderFactory)
        {
            this.command = command;
            this.requestBuilder = requestMessageBuilderFactory.GetRequestMessageBuilder<ReplicateRequestMessageBuilder>();
        }

        public HttpRequestMessage HttpRequestMessage => this.requestBuilder.GetRequestMessage(this.command);
    }
}
