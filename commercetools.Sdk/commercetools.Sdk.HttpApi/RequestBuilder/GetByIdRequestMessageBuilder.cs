namespace commercetools.Sdk.HttpApi
{
    using commercetools.Sdk.Client;
    using System;
    using System.Net.Http;

    public class GetByIdRequestMessageBuilder : RequestMessageBuilderBase
    {
        private GetByIdCommand command;

        public GetByIdRequestMessageBuilder(IClientConfiguration clientConfiguration) : base(clientConfiguration)
        {
        }

        public override Type CommandType => typeof(GetByIdCommand);
        protected override HttpContent HttpContent => null;
        protected override HttpMethod HttpMethod => HttpMethod.Get;

        public override HttpRequestMessage GetRequestMessage<T>(ICommand command)
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