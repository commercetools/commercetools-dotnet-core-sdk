using System.Net.Http;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain.OrderEdits;
using commercetools.Sdk.HttpApi.RequestBuilders;

namespace commercetools.Sdk.HttpApi.HttpApiCommands
{
   public class ApplyResourceEditHttpApiCommand<T> : IHttpApiCommandGeneric<ApplyResourceEditCommand<T>, T>
    {
        private readonly ApplyResourceEditCommand<T> command;
        private readonly ApplyResourceEditRequestMessageBuilder requestBuilder;

        public ApplyResourceEditHttpApiCommand(ApplyResourceEditCommand<T> command, IRequestMessageBuilderFactory requestMessageBuilderFactory)
        {
            this.command = command;
            this.requestBuilder = requestMessageBuilderFactory.GetRequestMessageBuilder<ApplyResourceEditRequestMessageBuilder>();
        }

        public HttpRequestMessage HttpRequestMessage => this.requestBuilder.GetRequestMessage(this.command);
    }
}
