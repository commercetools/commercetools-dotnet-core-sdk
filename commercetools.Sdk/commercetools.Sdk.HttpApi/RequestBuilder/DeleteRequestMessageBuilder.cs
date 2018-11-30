namespace commercetools.Sdk.HttpApi
{
    using commercetools.Sdk.Client;
    using System;
    using System.Net.Http;

    public class DeleteRequestMessageBuilder : RequestMessageBuilderBase, IRequestMessageBuilder
    {
        public DeleteRequestMessageBuilder(IClientConfiguration clientConfiguration, IEndpointRetriever endpointRetriever) : base(clientConfiguration, endpointRetriever)
        {
        }

        private HttpContent GetHttpContent()
        {
            return null;
        }

        private HttpMethod HttpMethod => HttpMethod.Delete;

        public HttpRequestMessage GetRequestMessage<T>(DeleteCommand<T> command)
        {
            return this.GetRequestMessage<T>(this.GetRequestUri<T>(command), null, this.HttpMethod);
        }

        private Uri GetRequestUri<T>(DeleteCommand<T> command)
        {
            string requestUri = this.GetMessageBase<T>();
            if (command.ParameterKey == Parameters.ID)
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