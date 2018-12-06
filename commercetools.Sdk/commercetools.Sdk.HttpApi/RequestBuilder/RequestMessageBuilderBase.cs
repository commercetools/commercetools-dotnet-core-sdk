namespace commercetools.Sdk.HttpApi
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using commercetools.Sdk.Domain;

    public abstract class RequestMessageBuilderBase
    {
        private readonly IClientConfiguration clientConfiguration;
        private readonly IEndpointRetriever endpointRetriever;
        private readonly IQueryStringRequestBuilderFactory queryStringRequestBuilderFactory;

        protected RequestMessageBuilderBase(IClientConfiguration clientConfiguration, IEndpointRetriever endpointRetriever, IQueryStringRequestBuilderFactory queryStringRequestBuilderFactory)
        {
            this.clientConfiguration = clientConfiguration;
            this.endpointRetriever = endpointRetriever;
            this.queryStringRequestBuilderFactory = queryStringRequestBuilderFactory;
        }

        protected string GetMessageBase<T>()
        {
            return this.clientConfiguration.ApiBaseAddress + $"{this.clientConfiguration.ProjectKey}/{this.endpointRetriever.GetEndpoint<T>()}";
        }

        protected List<KeyValuePair<string, string>> GetAdditionalParameters<T>(IAdditionalParameters<T> additionalParameters)
        {
            List<KeyValuePair<string, string>> queryStringParameters = new List<KeyValuePair<string, string>>();
            if (additionalParameters == null)
            {
                return queryStringParameters;
            }

            IQueryStringRequestBuilder<T> queryStringRequestBuilder = this.queryStringRequestBuilderFactory.GetQueryStringRequestBuilder<T>();
            queryStringParameters.AddRange(queryStringRequestBuilder.GetQueryStringParameters(additionalParameters));
            return queryStringParameters;
        }

        protected HttpRequestMessage GetRequestMessage<T>(Uri requestUri, HttpContent httpContent, HttpMethod httpMethod)
        {
            HttpRequestMessage request = new HttpRequestMessage
            {
                RequestUri = requestUri,
                Method = httpMethod
            };
            if (httpContent != null)
            {
                request.Content = httpContent;
            }

            return request;
        }
    }
}