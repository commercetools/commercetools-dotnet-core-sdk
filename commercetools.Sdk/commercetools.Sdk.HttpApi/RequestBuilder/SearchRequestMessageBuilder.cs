namespace commercetools.Sdk.HttpApi
{
    using commercetools.Sdk.Client;
    using System;
    using System.Net.Http;

    public class SearchRequestMessageBuilder : RequestMessageBuilderBase, IRequestMessageBuilder
    {
        public SearchRequestMessageBuilder(IClientConfiguration clientConfiguration) : base(clientConfiguration)
        {
        }

        protected override HttpMethod HttpMethod => HttpMethod.Post;

        public HttpRequestMessage GetRequestMessage<T>(SearchCommand<T> command)
        {
            return this.GetRequestMessage<T>(this.GetRequestUri<T>(), null);
        }

        private Uri GetRequestUri<T>()
        {
            string requestUri = this.GetMessageBase<T>() + "/";
            return new Uri(requestUri);
        }
    }
}