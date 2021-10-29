using System.Net.Http;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.HttpApi.RequestBuilders;

namespace commercetools.Sdk.HttpApi.HttpApiCommands
{
    public class CheckHttpApiCommand<T> : IHttpApiCommandGeneric<CheckCommand<T>, T>
    where T : Resource<T>, ICheckable<T>
    {
        private readonly CheckCommand<T> command;
        private readonly CheckRequestMessageBuilder requestBuilder;

        public CheckHttpApiCommand(CheckCommand<T> command, IRequestMessageBuilderFactory requestMessageBuilderFactory)
        {
            this.command = command;
            this.requestBuilder = requestMessageBuilderFactory.GetRequestMessageBuilder<CheckRequestMessageBuilder>();
        }

        public HttpRequestMessage HttpRequestMessage => this.requestBuilder.GetRequestMessage(this.command);
    }
}
