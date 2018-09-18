using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace commercetools.Sdk.HttpApi
{
    public abstract class RequestMessageBuilderBase : IRequestMessageBuilder
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

        public abstract Type CommandType { get; }
        protected abstract HttpContent HttpContent { get; }

        protected abstract HttpMethod HttpMethod { get; }

        public abstract HttpRequestMessage GetRequestMessage<T>(ICommand command);

        protected string GetMessageBase<T>()
        {
            return this.clientConfiguration.ApiBaseAddress + $"{this.clientConfiguration.ProjectKey}/{this.mapping[typeof(T)]}";
        }

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

        protected abstract Uri GetRequestUri<T>();
    }
}