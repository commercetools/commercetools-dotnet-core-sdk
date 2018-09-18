namespace commercetools.Sdk.HttpApi
{
    using commercetools.Sdk.Client;
    using System;
    using System.Net.Http;

    public class DeleteByIdRequestMessageBuilder : RequestMessageBuilderBase
    {
        public DeleteByIdRequestMessageBuilder(IClientConfiguration clientConfiguration) : base(clientConfiguration)
        {
        }

        private HttpContent GetHttpContent()
        {
            return null;
        }

        protected override HttpMethod HttpMethod => HttpMethod.Delete;

        public override HttpRequestMessage GetRequestMessage<T>(ICommand<T> command)
        {
            if (!(command is DeleteByIdCommand<T> deleteByIdCommand))
            {
                throw new InvalidCastException();
            }
            return this.GetRequestMessage<T>(this.GetRequestUri<T>(deleteByIdCommand), null);
        }

        private Uri GetRequestUri<T>(DeleteByIdCommand<T> command)
        {
            string requestUri = this.GetMessageBase<T>() + $"/{command.Guid}?version={command.Version}";
            return new Uri(requestUri);
        }
    }
}