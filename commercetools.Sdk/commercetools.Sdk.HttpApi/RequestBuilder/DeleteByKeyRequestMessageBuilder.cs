namespace commercetools.Sdk.HttpApi
{
    using commercetools.Sdk.Client;
    using System;
    using System.Net.Http;

    public class DeleteByKeyRequestMessageBuilder : RequestMessageBuilderBase, IRequestMessageBuilder
    {

        public DeleteByKeyRequestMessageBuilder(IClientConfiguration clientConfiguration) : base(clientConfiguration)
        {
        }

        protected override HttpMethod HttpMethod => HttpMethod.Delete;

        public HttpRequestMessage GetRequestMessage<T>(DeleteByKeyCommand<T> command)
        {
            return this.GetRequestMessage<T>(this.GetRequestUri<T>(command), null);
        }

        private Uri GetRequestUri<T>(DeleteByKeyCommand<T> command)
        {
            string requestUri = this.GetMessageBase<T>() + $"/key={command.Key}?version={command.Version}";
            return new Uri(requestUri);
        }
    }
}