namespace commercetools.Sdk.HttpApi
{
    using commercetools.Sdk.Client;
    using System;
    using System.Net.Http;

    public class GetByIdRequestMessageBuilder : RequestMessageBuilderBase
    {
        public GetByIdRequestMessageBuilder(IClientConfiguration clientConfiguration) : base(clientConfiguration)
        {
        }

        protected override HttpMethod HttpMethod => HttpMethod.Get;

        public override HttpRequestMessage GetRequestMessage<T>(ICommand<T> command)
        {
            if (!(command is GetByIdCommand<T> getByIdCommand))
            {
                throw new InvalidCastException();
            }
            return this.GetRequestMessage<T>(this.GetRequestUri<T>(getByIdCommand), null);
        }

        private Uri GetRequestUri<T>(GetByIdCommand<T> command)
        {
            string requestUri = this.GetMessageBase<T>() + $"/{command.Guid}";
            return new Uri(requestUri);
        }
    }
}