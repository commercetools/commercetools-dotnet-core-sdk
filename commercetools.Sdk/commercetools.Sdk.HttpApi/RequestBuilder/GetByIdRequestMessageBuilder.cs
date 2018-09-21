namespace commercetools.Sdk.HttpApi
{
    using commercetools.Sdk.Client;
    using System;
    using System.Net.Http;

    public class GetByIdRequestMessageBuilder : RequestMessageBuilderBase, IRequestMessageBuilder
    {
        public GetByIdRequestMessageBuilder(IClientConfiguration clientConfiguration) : base(clientConfiguration)
        {
        }

        protected override HttpMethod HttpMethod => HttpMethod.Get;

        public HttpRequestMessage GetRequestMessage<T>(GetByIdCommand<T> command)
        {
            return this.GetRequestMessage<T>(this.GetRequestUri<T>(command), null);
        }

        private Uri GetRequestUri<T>(GetByIdCommand<T> command)
        {
            string requestUri = this.GetMessageBase<T>() + $"/{command.Guid}";
            return new Uri(requestUri);
        }
    }
}