using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Type = System.Type;

namespace commercetools.Sdk.HttpApi
{
    public abstract class RequestMessageBuilderBase
    {
        private readonly IClientConfiguration clientConfiguration;
        private readonly IEndpointRetriever endpointRetriever;

        public RequestMessageBuilderBase(IClientConfiguration clientConfiguration, IEndpointRetriever endpointRetriever)
        {
            this.clientConfiguration = clientConfiguration;
            this.endpointRetriever = endpointRetriever;
        }

        protected string GetMessageBase<T>()
        {
            return this.clientConfiguration.ApiBaseAddress + $"{this.clientConfiguration.ProjectKey}/{this.endpointRetriever.GetEndpoint<T>()}";
        }

        protected HttpRequestMessage GetRequestMessage<T>(Uri requestUri, HttpContent httpContent, HttpMethod httpMethod)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = requestUri;
            request.Method = httpMethod;
            if (httpContent != null)
            {
                request.Content = httpContent;
            }
            return request;
        }
    }
}