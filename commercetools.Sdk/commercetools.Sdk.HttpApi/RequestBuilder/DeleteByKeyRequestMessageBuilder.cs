namespace commercetools.Sdk.HttpApi
{
    using commercetools.Sdk.Client;
    using System;
    using System.Net.Http;

    public class DeleteByKeyRequestMessageBuilder : RequestMessageBuilderBase
    {

        public DeleteByKeyRequestMessageBuilder(IClientConfiguration clientConfiguration) : base(clientConfiguration)
        {
        }

        protected override HttpMethod HttpMethod => HttpMethod.Delete;

        public override HttpRequestMessage GetRequestMessage<T>(ICommand<T> command)
        {
            if (!(command is DeleteByKeyCommand<T> deleteByKeyCommand))
            {
                throw new InvalidCastException();
            }
            return this.GetRequestMessage<T>(this.GetRequestUri<T>(deleteByKeyCommand), null);
        }

        private Uri GetRequestUri<T>(DeleteByKeyCommand<T> command)
        {
            string requestUri = this.GetMessageBase<T>() + $"/key={command.Key}?version={command.Version}";
            return new Uri(requestUri);
        }
    }
}