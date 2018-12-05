namespace commercetools.Sdk.HttpApi
{
    using commercetools.Sdk.Client;
    using Microsoft.AspNetCore.WebUtilities;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;

    public class DeleteRequestMessageBuilder : RequestMessageBuilderBase, IRequestMessageBuilder
    {
        public DeleteRequestMessageBuilder(IClientConfiguration clientConfiguration, 
            IEndpointRetriever endpointRetriever, 
            IQueryStringRequestBuilderFactory queryStringRequestBuilderFactory) : base(clientConfiguration, endpointRetriever, queryStringRequestBuilderFactory)
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
            if (command.ParameterKey == Parameters.Id)
            {
                requestUri += $"/{command.ParameterValue}";
            }
            else
            {
                requestUri += $"/{command.ParameterKey}={command.ParameterValue}";
            }
            requestUri += $"?version={command.Version}";
            List<KeyValuePair<string, string>> queryStringParameters = new List<KeyValuePair<string, string>>();
            queryStringParameters.AddRange(this.GetAdditionalParameters(command.AdditionalParameters));
            queryStringParameters.ForEach(x => { requestUri = QueryHelpers.AddQueryString(requestUri, x.Key, x.Value); });
            return new Uri(requestUri);
        }
    }
}