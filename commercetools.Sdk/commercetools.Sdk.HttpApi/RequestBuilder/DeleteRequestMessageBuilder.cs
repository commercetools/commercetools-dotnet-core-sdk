namespace commercetools.Sdk.HttpApi
{
    using commercetools.Sdk.Client;
    using System;
    using System.Net.Http;

    public class DeleteRequestMessageBuilder : RequestMessageBuilderBase, IRequestMessageBuilder
    {
        public DeleteRequestMessageBuilder(IClientConfiguration clientConfiguration) : base(clientConfiguration)
        {
        }

        private HttpContent GetHttpContent()
        {
            return null;
        }

        protected override HttpMethod HttpMethod => HttpMethod.Delete;

        public HttpRequestMessage GetRequestMessage<T>(DeleteCommand<T> command)
        {
            return this.GetRequestMessage<T>(this.GetRequestUri<T>(command), null);
        }

        private Uri GetRequestUri<T>(DeleteCommand<T> command)
        {
            string requestUri = this.GetMessageBase<T>();
            if (command.ParameterKey == "id")
            {
                requestUri += $"/{command.ParameterValue}";
            }
            else
            {
                requestUri += $"/{command.ParameterKey}={command.ParameterValue}";
            }
            requestUri += $"?version={command.Version}";
            return new Uri(requestUri);
        }
    }
}