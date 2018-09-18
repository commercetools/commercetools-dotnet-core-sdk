namespace commercetools.Sdk.HttpApi
{
    using commercetools.Sdk.Client;
    using System;
    using System.Net.Http;

    public class DeleteByKeyRequestMessageBuilder : RequestMessageBuilderBase
    {
        private DeleteByKeyCommand command;

        public DeleteByKeyRequestMessageBuilder(IClientConfiguration clientConfiguration) : base(clientConfiguration)
        {
        }

        public override Type CommandType => typeof(DeleteByKeyCommand);
        protected override HttpContent HttpContent => null;
        protected override HttpMethod HttpMethod => HttpMethod.Delete;

        public override HttpRequestMessage GetRequestMessage<T>(ICommand command)
        {
            this.command = command as DeleteByKeyCommand;
            return this.GetRequestMessage<T>();
        }

        protected override Uri GetRequestUri<T>()
        {
            string requestUri = this.GetMessageBase<T>() + $"/key={this.command.Key}?version={this.command.Version}";
            return new Uri(requestUri);
        }
    }
}