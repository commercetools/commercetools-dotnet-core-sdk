namespace commercetools.Sdk.HttpApi
{
    using commercetools.Sdk.Client;
    using commercetools.Sdk.Serialization;
    using System.Net.Http;

    public class RequestBuilder : IRequestBuilder
    {
        private readonly IRequestMessageBuilderFactory requestMessageBuilderFactory;

        public RequestBuilder(IRequestMessageBuilderFactory requestMessageBuilderFactory)
        {
            this.requestMessageBuilderFactory = requestMessageBuilderFactory;
        }

        public HttpRequestMessage GetRequestMessage<T>(ICommand<T> command)
        {
            // TODO Add a null pointer check
            IRequestMessageBuilder requestMessageBuilder = this.requestMessageBuilderFactory.GetRequestMessageBuilder(command);
            return requestMessageBuilder.GetRequestMessage<T>(command);
        }
    }
}