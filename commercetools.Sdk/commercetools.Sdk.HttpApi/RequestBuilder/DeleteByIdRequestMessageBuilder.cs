namespace commercetools.Sdk.HttpApi
{
    using commercetools.Sdk.Client;
    using System;
    using System.Net.Http;

    public class DeleteByIdRequestMessageBuilder : RequestMessageBuilderBase, IRequestMessageBuilder
    {
        public DeleteByIdRequestMessageBuilder(IClientConfiguration clientConfiguration) : base(clientConfiguration)
        {
        }

        private HttpContent GetHttpContent()
        {
            return null;
        }

        protected override HttpMethod HttpMethod => HttpMethod.Delete;

        public HttpRequestMessage GetRequestMessage<T>(DeleteByIdCommand<T> command)
        {
            return this.GetRequestMessage<T>(this.GetRequestUri<T>(command), null);
        }

        private Uri GetRequestUri<T>(DeleteByIdCommand<T> command)
        {
            string requestUri = this.GetMessageBase<T>() + $"/{command.Guid}?version={command.Version}";
            return new Uri(requestUri);
        }
    }
}