using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace commercetools.Sdk.HttpApi
{
    public abstract class RequestMessageBuilderBase
    {
        // TODO See if this should be moved to a different class
        private IDictionary<Type, string> mapping = new Dictionary<Type, string>()
        {
            {  typeof(Category), "categories" }
        };

        protected string GetMessageBase<T>()
        {
            return this.clientConfiguration.ApiBaseAddress + $"{this.clientConfiguration.ProjectKey}/{this.mapping[typeof(T)]}";
        }

        private IClientConfiguration clientConfiguration;

        public RequestMessageBuilderBase(IClientConfiguration clientConfiguration)
        {
            this.clientConfiguration = clientConfiguration;
        }

        protected abstract Uri GetRequestUri<T>();
        protected abstract HttpMethod HttpMethod { get; }
        protected abstract HttpContent HttpContent { get; }

        protected HttpRequestMessage GetRequestMessage<T>()
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = this.GetRequestUri<T>();
            request.Method = this.HttpMethod;
            if (this.HttpContent != null)
            {
                request.Content = this.HttpContent;
            }
            return request;
        }
    }
}
