namespace commercetools.Sdk.HttpApi
{
    using commercetools.Sdk.Client;
    using commercetools.Sdk.Linq;
    using Microsoft.AspNetCore.WebUtilities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    public class GetRequestMessageBuilder : RequestMessageBuilderBase, IRequestMessageBuilder
    {
        public GetRequestMessageBuilder(
            IClientConfiguration clientConfiguration, 
            IEndpointRetriever endpointRetriever,
            IQueryStringRequestBuilderFactory queryStringRequestBuilderFactory) : base(clientConfiguration, endpointRetriever, queryStringRequestBuilderFactory)
        {
        }

        private HttpMethod HttpMethod => HttpMethod.Get;

        public HttpRequestMessage GetRequestMessage<T>(GetCommand<T> command)
        {
            return this.GetRequestMessage<T>(this.GetRequestUri(command), null, this.HttpMethod);
        }

        private Uri GetRequestUri<T>(GetCommand<T> command)
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

            List<KeyValuePair<string, string>> queryStringParameters = new List<KeyValuePair<string, string>>();
            if (command.Expand != null)
            {
                queryStringParameters.AddRange(command.Expand.Select(x => new KeyValuePair<string, string>("expand", x)));
            }

            queryStringParameters.AddRange(this.GetAdditionalParameters(command.AdditionalParameters));
            queryStringParameters.ForEach(x => { requestUri = QueryHelpers.AddQueryString(requestUri, x.Key, x.Value); });
            return new Uri(requestUri);
        }
    }
}