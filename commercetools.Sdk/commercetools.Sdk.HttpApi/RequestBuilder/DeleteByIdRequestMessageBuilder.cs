namespace commercetools.Sdk.HttpApi
{
    using commercetools.Sdk.Client;
    using System;
    using System.Net.Http;

    public class DeleteByIdRequestMessageBuilder : RequestMessageBuilderBase
    {
        private DeleteByIdCommand command;

        public DeleteByIdRequestMessageBuilder(IClientConfiguration clientConfiguration) : base(clientConfiguration)
        {
        }

        public override Type CommandType => typeof(DeleteByIdCommand);
        protected override HttpContent HttpContent => null;
        protected override HttpMethod HttpMethod => HttpMethod.Delete;

        public override HttpRequestMessage GetRequestMessage<T>(ICommand command)
        {
            this.command = command as DeleteByIdCommand;
            return this.GetRequestMessage<T>();
        }

        protected override Uri GetRequestUri<T>()
        {
            string requestUri = this.GetMessageBase<T>() + $"/{this.command.Guid}?version={this.command.Version}";
            return new Uri(requestUri);
        }
    }
}