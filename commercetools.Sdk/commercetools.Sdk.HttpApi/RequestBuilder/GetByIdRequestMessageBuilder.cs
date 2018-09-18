namespace commercetools.Sdk.HttpApi
{
    using commercetools.Sdk.Client;
    using System;
    using System.Net.Http;

    public class GetByIdRequestMessageBuilder : RequestMessageBuilderBase, IRequestMessageBuilder
    {
        public Type CommandType => typeof(GetByIdCommand);
        private GetByIdCommand command;

        public GetByIdRequestMessageBuilder(IClientConfiguration clientConfiguration) : base(clientConfiguration)
        {
        }

        public string RequestUriEnd => $"/{this.command.Guid}";

        protected override HttpContent HttpContent => null;
        protected override HttpMethod HttpMethod => HttpMethod.Get;

        public HttpRequestMessage GetRequestMessage<T>(ICommand command)
        {
            this.command = command as GetByIdCommand;
            return this.GetRequestMessage<T>();
        }

        protected override Uri GetRequestUri<T>()
        {
            string requestUri = this.GetMessageBase<T>() + $"/{this.command.Guid}";
            return new Uri(requestUri);
        }
    }
}