namespace commercetools.Sdk.HttpApi
{
    using commercetools.Sdk.Client;
    using System;
    using System.Net.Http;

    public class GetByKeyRequestMessageBuilder : RequestMessageBuilderBase
    {
        public GetByKeyRequestMessageBuilder(IClientConfiguration clientConfiguration) : base(clientConfiguration)
        {
        }

        protected override HttpMethod HttpMethod => HttpMethod.Get;

        public override HttpRequestMessage GetRequestMessage<T>(ICommand<T> command)
        {
            if (!(command is GetByKeyCommand<T> getByKeyCommand))
            {
                throw new InvalidCastException();
            }
            return this.GetRequestMessage<T>(this.GetRequestUri<T>(getByKeyCommand), null);
        }

        private Uri GetRequestUri<T>(GetByKeyCommand<T> command)
        {
            string requestUri = this.GetMessageBase<T>() + $"/key={command.Key}";
            return new Uri(requestUri);
        }
    }
}