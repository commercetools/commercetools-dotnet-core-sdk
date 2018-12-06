using commercetools.Sdk.Client;
using System.Net.Http;

namespace commercetools.Sdk.HttpApi
{
    public class UpdateHttpApiCommand<T> : IHttpApiCommand<UpdateCommand<T>, T>
    {
        private readonly UpdateCommand<T> command;
        private readonly UpdateRequestMessageBuilder requestBuilder;

        public UpdateHttpApiCommand(UpdateCommand<T> command, IRequestMessageBuilderFactory requestMessageBuilderFactory)
        {
            this.command = command;
            this.requestBuilder = requestMessageBuilderFactory.GetRequestMessageBuilder<UpdateRequestMessageBuilder>();
        }

        public HttpRequestMessage HttpRequestMessage => this.requestBuilder.GetRequestMessage(this.command);
    }
}