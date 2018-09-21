using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace commercetools.Sdk.HttpApi
{
    public abstract class RequestMessageBuilderBase
    {
        private IClientConfiguration clientConfiguration;

        // TODO See if this should be moved to a different class
        private IDictionary<Type, string> mapping = new Dictionary<Type, string>()
        {
            {  typeof(Category), "categories" }
        };

        public RequestMessageBuilderBase(IClientConfiguration clientConfiguration)
        {
            this.clientConfiguration = clientConfiguration;
        }

        protected abstract HttpMethod HttpMethod { get; }

        protected string GetMessageBase<T>()
        {
            return this.clientConfiguration.ApiBaseAddress + $"{this.clientConfiguration.ProjectKey}/{this.mapping[typeof(T)]}";
        }

        protected HttpRequestMessage GetRequestMessage<T>(Uri requestUri, HttpContent httpContent)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = requestUri;
            request.Method = this.HttpMethod;
            if (httpContent != null)
            {
                request.Content = httpContent;
            }
            return request;
        }
    }
}