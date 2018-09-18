namespace commercetools.Sdk.HttpApi
{
    using commercetools.Sdk.Client;
    using System;
    using System.Net.Http;

    public class GetByKeyRequestMessageBuilder : RequestMessageBuilderBase, IRequestMessageBuilder
    {
        public Type CommandType => typeof(GetByKeyCommand);
        private GetByKeyCommand command;

        public GetByKeyRequestMessageBuilder(IClientConfiguration clientConfiguration) : base(clientConfiguration)
        {
        }

        protected override HttpContent HttpContent => null;
        protected override HttpMethod HttpMethod => HttpMethod.Get;

        public HttpRequestMessage GetRequestMessage<T>(ICommand command)
        {
            this.command = command as GetByKeyCommand;
            return this.GetRequestMessage<T>();
        }

        protected override Uri GetRequestUri<T>()
        {
            string requestUri = this.GetMessageBase<T>() + $"/key={this.command.Key}";
            return new Uri(requestUri);
        }
    }
}